using UnityEngine;

using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NWorld;
using Assets.Scripts.NEvent;
using Assets.Scripts.NData;


namespace Assets.Scripts.NCharacter
{
    [RequireComponent(typeof(Communicator))]
    class ChaCollider : MonoBehaviour
    {
        private IWorld m_refWorld;

        //private ChaPosition m_ChaPosition;

        //private Vector3Int
        [Range(0.1f, 0.9f)]
        public float m_BodyWidth = 0.8f;
        [Range(1, 3)]
        public float m_BodyHeight = 1.8f;

        private float m_BodyWidthExtent;
        private float m_BodyHeightExtent;

        public float m_Gravity = 9.8f;

        public float m_DownSpeed = 0.0f;

        public bool m_IsGround = false;



        //Unity Function
        //---------------------------------------------------------------------
        private void Start()
        {
            GetComponent<Communicator>().SubsribeEvent_Decorate(E_Cha_MoveRequest.ID, AvoidIntersect);

            //Handle jump request
            GetComponent<Communicator>().SubsribeEvent(E_Cha_Jump.ID, TriggerJump);

            m_refWorld = Locator<IWorld>.GetService();

            //Init Body Bound Data
            m_BodyWidthExtent = m_BodyWidth / 2;
            m_BodyHeightExtent = m_BodyHeight / 2;
        }


        private void Update()
        {
            if (m_IsGround == true)
            {
                if (Down(-0.1f, out Vector3 intersection) == false)
                {
                    m_IsGround = false;
                }
            }

            if (m_IsGround == false)
            {
                m_DownSpeed -= m_Gravity * Time.deltaTime;

                //Limit speed for AABB collide detection
                if (m_DownSpeed > 1.0f) m_DownSpeed = 1.0f;
                if (m_DownSpeed < -1.0f) m_DownSpeed = -1.0f;

                CheckUpDown();
            }
        }


        //intersection Handler
        //---------------------------------------------------------------------
        private IEvent AvoidIntersect(IEvent _event)
        {
            Vector3 Translation = (_event as E_Cha_MoveRequest).Translation;

            Vector3 Intersection;
            if (Translation.z > 0 && Front(Translation.z,out Intersection))
            {
                Translation -= Intersection;
            }

            else if(Translation.z < 0 && Back(Translation.z, out Intersection))
            {
                Translation += Intersection;
            }

            if (Translation.x > 0 && Right(Translation.x, out Intersection))
            {
                Translation -= Intersection;
            }
            else if (Translation.x < 0 && Left(Translation.x, out Intersection))
            {
                Translation += Intersection;
            }
            return new E_Cha_MoveRequest(Translation);            
        }


        //Jump request Handler
        private bool TriggerJump(IEvent _event)
        {
            E_Cha_Jump EJump = _event as E_Cha_Jump;
            m_DownSpeed = EJump.Force;
            m_IsGround = false;

            return true;
        }


        //Check if there is a solid block in front of me
        private bool Front(float trans_z,out Vector3 intersect)
        {
            intersect = Vector3.zero;

            for (int i = 0; i < m_BodyHeight; ++i)
            {
                BlockLocation Loc = new BlockLocation(transform.position + new Vector3(0, i, m_BodyWidthExtent + trans_z), m_refWorld);

                if (Loc.IsBlockExists() && Loc.CurBlockRef.IsObstacle)
                {
                    intersect.z = transform.position.z + trans_z + m_BodyWidthExtent- Loc.Bound.min.z;
                    //Debug.Log("front intersection"+ intersect);
                    return true;
                } 
            }

            return false;
        }
        private bool Back(float trans_z, out Vector3 intersect)
        {
            intersect = Vector3.zero;

            for (int i = 0; i < m_BodyHeight; ++i)
            {
                BlockLocation Loc = new BlockLocation(transform.position + new Vector3(0, i, -m_BodyWidthExtent + trans_z), m_refWorld);

                if (Loc.IsBlockExists() && Loc.CurBlockRef.IsObstacle)
                {
                    intersect.z = Loc.Bound.max.z - (transform.position.z + trans_z - m_BodyWidthExtent);
                    //Debug.Log("back intersection" + intersect);
                    return true;
                }
            }
            return false;
        }
        private bool Left(float trans_x, out Vector3 intersect)
        {
            intersect = Vector3.zero;
            for (int i = 0; i < m_BodyHeight; ++i)
            {
                BlockLocation Loc = new BlockLocation(transform.position + new Vector3(-m_BodyWidthExtent + trans_x, i, 0), m_refWorld);

                if (Loc.IsBlockExists() && Loc.CurBlockRef.IsObstacle)
                {
                    intersect.x = Loc.Bound.max.x-(transform.position.x + trans_x - m_BodyWidthExtent);
                    //Debug.Log("left intersection" + intersect);
                    return true;
                }
            }
            return false;
        }
        private bool Right(float trans_x, out Vector3 intersect)
        {
            intersect = Vector3.zero;
            for (int i = 0; i < m_BodyHeight; ++i)
            {
                BlockLocation Loc = new BlockLocation
                    (transform.position + new Vector3(m_BodyWidthExtent + trans_x, i, 0), m_refWorld);
                
                if (Loc.IsBlockExists() && Loc.CurBlockRef.IsObstacle)
                {
                    intersect.x = (transform.position.x + trans_x + m_BodyWidthExtent) - Loc.Bound.min.x;
                    //Debug.Log("right intersection" + intersect);
                    return true;
                }
            }
            return false;
        }

        private bool Down(float trans_y, out Vector3 intersect)
        {
            intersect = Vector3.zero;
            BlockLocation Loc =
                new BlockLocation(transform.position + new Vector3(0, trans_y - m_BodyHeightExtent, 0), m_refWorld);

            if (Loc.IsBlockExists() && Loc.CurBlockRef.IsObstacle)
            {
                intersect.y = Loc.Bound.max.y - (transform.position.y + trans_y - m_BodyHeightExtent);
                //Debug.Log("Down intersection" + intersect);
                return true;
            }
            return false;
        }
        private bool Up(float trans_y, out Vector3 intersect)
        {
            intersect = Vector3.zero;
            BlockLocation Loc =
                new BlockLocation(transform.position + new Vector3(0, trans_y + m_BodyHeightExtent, 0), m_refWorld);

            if (Loc.IsBlockExists() && Loc.CurBlockRef.IsObstacle)
            {
                intersect.y = Loc.Bound.min.y - (transform.position.y + trans_y + m_BodyHeight);
                //Debug.Log("Up intersection" + intersect);
                return true;
            }
            return false;
        }

        private void CheckUpDown()
        {       
            if (m_DownSpeed > 0)
            {
                //check up block
                Vector3 intersection = Vector3.zero;
                if (Up(m_DownSpeed, out intersection))
                {
                    m_DownSpeed = 0;
                }

                transform.position += new Vector3(0, m_DownSpeed, 0) - intersection;
            }

            else if (m_DownSpeed < 0)
            {
                Vector3 intersection = Vector3.zero;
                if (Down(m_DownSpeed, out intersection))
                {
                    m_IsGround = true;
                    //Debug.Log("touch ground");
                }

                transform.position = transform.position + new Vector3(0, m_DownSpeed, 0) + intersection;
                
            }
        }

 
    }

}

 


