using UnityEngine;
using Assets.Scripts.NWorld;
using Assets.Scripts.NEvent;
using Assets.Scripts.NEvent.Impl;
using Assets.Scripts.NServiceLocator;

namespace Assets.Scripts.NUser
{
    class Cursor : MonoBehaviour
    {
        public GameObject go_placeFrame;
        public GameObject go_PickFrame;


        [SerializeField]
        private int m_HandLength;

        //Cache
        //----------------------------------
        private IWorld m_refWorld;
        private Ray m_Ray;

        [SerializeField]
        private BlockLocation m_Picked;
        [SerializeField]
        private BlockLocation m_Placed;

        private Vector3 m_Checking;
        private Collider m_Check_bound;

        //testing
        public  Vector3 m_offset;

        private void Awake()
        {
            m_refWorld = Locator<IWorld>.GetService();
            m_Check_bound = go_PickFrame.GetComponent<BoxCollider>();
        }
        private void Start()
        {
            
        }
        private void FixedUpdate()
        {
            //ChangeBlock();
            PlaceBlock();
        }

        private void ChangeBlock() 
        {
            if (PickBlock())
            {
                //click
                if (Input.GetMouseButtonDown(0))
                {
                    //!!!!!!!!!!!!!!!!!!!!!!!!!
                    m_Picked.CurBlockID = 3;
                   //Locator<IEventPublisher>.GetService().Publish(new E_Block_Change(ref m_Picked,0));
                }
            }
        } 

        void PlaceBlock()
        {
            if (PickBlockSide())
            {
                //click
                if (Input.GetMouseButtonDown(0))
                {
                    m_Placed.CurBlockID = 3;
                    Debug.Log("pick suceed!");
                }
            }
        }

        bool PickBlock()
        {
            int distance = m_HandLength;

            //cast ray from mouse position
            m_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            m_Checking = m_Ray.origin;

            //test
            do
            {
                m_Checking += m_Ray.direction;
                
                m_Picked.Set(m_Checking, m_refWorld);
                if (m_Picked.IsValid())
                {
                    go_PickFrame.SetActive(true);
                    go_PickFrame.transform.position = m_Picked.Bound.center;
                    return true;
                }
            }
            while (--distance > 0);

            go_PickFrame.SetActive(false);
            return false;
        }

        bool PickBlockSide()
        {
            //Case 
            if (PickBlock())
            {
                if (CheckHitSide(m_Ray))
                {
                    m_Placed.Set(m_Checking + m_offset, m_refWorld);

                    go_placeFrame.SetActive(true);
                    go_placeFrame.transform.position = m_Placed.Bound.center;
                  
                    return true;
                }
            }

            go_placeFrame.SetActive(false);
            return false;
        }

        bool CheckHitSide(Ray _ray)
        {
            RaycastHit rayhit;
            if (m_Check_bound.Raycast(_ray, out rayhit, m_HandLength))
            {
                if (rayhit.normal == Vector3.zero) return false;

                //Debug.Log("hit offset: " + m_offset);
                m_offset = rayhit.normal;
                return true;
            }
            return false;
        }

        void AABB_RAY(Bounds bound, Ray ray)
        {
            // target = ray_origin + ray_dir * T

            // intersect plane x min

            // (0,xmin,0) =  ray_origin + ray_dir * T
            // => T = ((0,xmin,0)-ray_origin )/ ray dir;
            // => intersected point = rayorigin + ray_dir * T;





        }


    }
}
