using UnityEngine;
using Assets.Scripts.NWorld;
using Assets.Scripts.NInput;
using Assets.Scripts.NEvent;

using Assets.Scripts.NGlobal.ServiceLocator;

namespace Assets.Scripts.NUser
{
    class Cursor : MonoBehaviour
    {
        public GameObject Go_Place;
        public GameObject Go_Pick;

        [SerializeField]
        [Range(3,8)]
        private int m_HandLength = 8;

        [SerializeField]
        [Range(0.25f, 1.0f)]
        private float m_RayCheckInterval = 0.5f;
        //Cache
        //----------------------------------
        private IWorld m_refWorld;
        private IController m_refControl;
        private IEventPublisher m_refEventPublisher;

        [SerializeField]
        private BlockLocation m_PickedLoc;  //the location of block is picked
        [SerializeField]
        private BlockLocation m_PlacedLoc;  //the location that block will be placed


        private void Awake()
        {
            m_refWorld = Locator<IWorld>.GetService();
            m_refControl = Locator<IController>.GetService();
            m_refEventPublisher = Locator<IEventPublisher>.GetService();
        }

        private void Update()
        {
            //ChangeBlock();
            _PlaceBlock(2);
            _UndoBlockChange();
        }

        private void _UndoBlockChange()
        {
            if (m_refControl.Back())
            {
                m_refEventPublisher.Publish(new E_Block_Recover());
            }
        }

        private void _SetBlock(byte blkType)
        {
            if (m_refControl.CursorDown())
            {
                //cast ray from cursor position
                Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (_IsBlockPicked(ref _ray))
                {
                    //publish event : block Change request
                    m_refEventPublisher.Publish(new E_Block_Change(ref m_PickedLoc, blkType));
                }
            }
        }
        private void _DestroyBlock()
        {
            _SetBlock(0);
        }
        //
        private void _PlaceBlock(byte blkType)
        {
            if (m_refControl.CursorDown())
            {
                Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (_IsBlockPicked(ref _ray))
                {
                    Vector3Int HitSide;
                    if (_CheckRayHitSide(_ray, out HitSide))
                    {
                        m_PlacedLoc = m_PickedLoc.Offset_Blk(HitSide, m_refWorld);

                        //there is no block located
                        if (!m_PlacedLoc.IsValid())
                        {
                            //publish event : block Change request
                            m_refEventPublisher.Publish(new E_Block_Change(ref m_PlacedLoc, blkType));

                            Go_Place.SetActive(true);
                            Go_Place.transform.position = m_PlacedLoc.Bound.center;
                            return;
                        }
                    }
                    Go_Place.SetActive(false);
                    return;
                }
            }
        }

        //Check if Block had been Picked
        private bool _IsBlockPicked(ref Ray _ray)
        {
            float distance = m_HandLength;

            Vector3 CheckLoc = _ray.origin;

            //test
            do
            {
                //Get Cur location
                CheckLoc += _ray.direction* m_RayCheckInterval;

                //Get Location Data
                m_PickedLoc.Set(CheckLoc, m_refWorld);

                //Check if Block is Picked
                if (m_PickedLoc.IsValid())
                {
                    Go_Pick.SetActive(true);
                    Go_Pick.transform.position = m_PickedLoc.Bound.center;
                    return true;
                }

                //Move to Next Location
                distance -= m_RayCheckInterval;
            }
            while (distance > 0);

            //Picking was Failed, disactive pick cursor UI 
            Go_Pick.SetActive(false);
            return false;
        }

        //Check which side of bound(box) hitted by ray
        private bool _CheckRayHitSide(Ray _ray, out Vector3Int Out_Offset)
        {
            Out_Offset = Vector3Int.zero;

            if (m_PickedLoc.Bound.IntersectRay(_ray, out float distance))
            {
                 Vector3 test_HitPoint = _ray.origin + distance * _ray.direction;
                 Vector3 Dir = test_HitPoint - m_PickedLoc.Bound.center;

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
    }
}
