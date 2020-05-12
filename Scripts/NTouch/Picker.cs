using UnityEngine;

using Assets.Scripts.NWorld;
using Assets.Scripts.NInput;
using Assets.Scripts.NEvent;
using Assets.Scripts.NData;
using Assets.Scripts.NGlobal.ServiceLocator;


namespace Assets.Scripts.NTouch
{
    public enum EnPickerMode {eNormal, eSet, ePut, eDestroy }

    public class Picker : MonoBehaviour
    {
        public GameObject Prefab_PutCursor;
        public GameObject Prefab_PickCursor;

        private GameObject Ins_PutCursor;
        private GameObject Ins_PickCursor;

        [SerializeField]
        [Range(3,16)]
        private int m_HandLength = 8;

        [SerializeField]
        [Range(0.25f, 1.0f)]
        private float m_RayCheckInterval = 1f;

        private bool m_bIsPickValid = false;
        private bool m_bIsPutValid = false;
        //private bool m_bHasPlayerSpawned = false;

        [SerializeField]
        private BlockLocation m_PickLoc;  //the location of block is picked

        [SerializeField]
        private BlockLocation m_PutLoc;  //the location that block will be putted

        Ray m_Ray;

        public EnPickerMode PickerMode { set; get; }
        public byte BlockSelected { set; get; }

        //Cache
        //----------------------------------
        private IWorld m_refWorld;
        private IController m_refControl;
        private IEventPublisher m_refEventPublisher;

        private void Awake()
        {
            //Cursor.lockState = CursorLockMode.Locked;

            Ins_PutCursor = Instantiate(Prefab_PutCursor);
            Ins_PickCursor = Instantiate(Prefab_PickCursor);

            PickerMode = EnPickerMode.ePut;
            BlockSelected = 1;
        }
        private void Start()
        {
            m_refWorld = Locator<IWorld>.GetService();
            m_refControl = Locator<IController>.GetService();
            m_refEventPublisher = Locator<IEventPublisher>.GetService();
        }

        private void Update()
        {
            if (PickerMode == EnPickerMode.eNormal) return;
            //if (m_bHasPlayerSpawned == false) return;

            //check weather mouse has moved
            if (m_refControl.HasCursorMoved())
            {
                _CheckCursorLocation();

                if (m_bIsPickValid && (PickerMode == EnPickerMode.eSet || PickerMode == EnPickerMode.eDestroy))
                {
                    Ins_PickCursor.SetActive(true);
                    Ins_PickCursor.transform.position = m_PickLoc.Bound.center;
                }
                else
                {
                    Ins_PickCursor.SetActive(false);
                }

                if (m_bIsPutValid && PickerMode == EnPickerMode.ePut)
                {
                    Ins_PutCursor.SetActive(true);
                    Ins_PutCursor.transform.position = m_PutLoc.Bound.center;
                }
                else
                {
                    Ins_PutCursor.SetActive(false);
                }
            }

            if (m_refControl.CursorDown())
            {
                switch (PickerMode)
                {
                    case EnPickerMode.eSet: _SetBlock(BlockSelected);
                        break;
                    case EnPickerMode.ePut:_PutBlock(BlockSelected);
                        break;
                    case EnPickerMode.eDestroy: _SetBlock(0);
                        break;
                    default:
                        break;
                }
            }

            if (m_refControl.Back())
            {
                UndoBlockModify();
            }
        }

        public void UndoBlockModify()
        {
            m_refEventPublisher.Publish(new E_Block_Recover());
        }

        private void _SetBlock(byte blkType)
        {
            if (m_bIsPickValid)
            {
                //publish event : set block  request
                m_refEventPublisher.Publish(new E_Block_Modify(ref m_PickLoc, blkType));
            }
        }
        private void _PutBlock(byte blkType)
        {
            //publish event : put block  request
            if (m_bIsPutValid)
            {
                m_refEventPublisher.Publish(new E_Block_Modify(ref m_PutLoc, blkType));
                m_bIsPutValid = false;
            }
        }
        private void _DestroyBlock()
        {
            _SetBlock(0);
        }


        private void _CheckCursorLocation()
        {
            m_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            _CheckPickLocation();

            if (m_bIsPickValid)
            {
                if (_CheckRayHitSide(out Vector3Int HitSide))
                {
                    m_PutLoc = m_PickLoc.Offset_Blk(HitSide, m_refWorld);

                    m_bIsPutValid = m_PutLoc.IsPutable(m_refWorld);
                }
                else m_bIsPutValid = false;
            }
            else m_bIsPutValid = false;

        }

        //Check if Block had been Picked
        private void _CheckPickLocation()
        {
            float distance = m_HandLength;
            Vector3 CheckLoc = m_Ray.origin;

            do
            {
                //Get Cur location
                CheckLoc += m_Ray.direction * m_RayCheckInterval;

                //Get Location Data
                m_PickLoc.Update(CheckLoc, m_refWorld);

                //Check if Block is Picked
                if (m_PickLoc.IsBlockExists())
                {
                    m_bIsPickValid = true;
                    return;
                }

                //Move to Next Location
                distance -= m_RayCheckInterval;
            }
            while (distance > 0);

            m_bIsPickValid = false;
        }

        //Check which side of bound(box) hitted by ray
        private bool _CheckRayHitSide(out Vector3Int Out_Offset)
        {
            Out_Offset = Vector3Int.zero;

            if (m_PickLoc.Bound.IntersectRay(m_Ray, out float distance))
            {
                 Vector3 test_HitPoint = m_Ray.origin + distance * m_Ray.direction;
                 Vector3 Dir = test_HitPoint - m_PickLoc.Bound.center;

                float absx = Mathf.Abs(Dir.x);
                float absy = Mathf.Abs(Dir.y);
                float absz = Mathf.Abs(Dir.z);

                if (absx > absy && absx > absz)
                {
                    Out_Offset = Dir.x > 0 ? Vector3Int.right : Vector3Int.left;
                    return true;
                }
                else if (absy > absx && absy > absz)
                {
                    Out_Offset = Dir.y > 0 ? Vector3Int.up : Vector3Int.down;
                    return true;
                }
                else if (absz > absx && absz > absy)
                {
                    Out_Offset = Dir.z > 0 ? new Vector3Int(0, 0, 1) : new Vector3Int(0, 0, -1);
                    return true;
                }
                else return false;
            }
            else return false; 
        }

        private bool Handle_PlayerSpawn()
        {
            //m_bHasPlayerSpawned = true;

            return true;
        }
    }
}
