using UnityEngine;

using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NWorld;
using Assets.Scripts.NEvent;
using Assets.Scripts.NData;


namespace Assets.Scripts.NCharacter
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(Communicator))]
    class ChaCollider : MonoBehaviour
    {
        private IWorld m_World;
        private Character m_Character;
        private Communicator m_Communicator;

        private float m_BodyWidth;
        private float m_BodyHeight;
        private float m_BodyWidth_Half;
        private float m_BodyHeight_Half;     

        //Unity Function
        //---------------------------------------------------------------------
        private void Awake()
        {
            m_Character = GetComponent<Character>();
            m_Communicator = GetComponent<Communicator>();

            //get body size
            m_BodyWidth = m_Character.BodyWidth;
            m_BodyHeight = m_Character.BodyHeight;
            m_BodyWidth_Half = m_BodyWidth / 2;
            m_BodyHeight_Half = m_BodyHeight / 2;
        }

        private void Start()
        {
            //avoid intersection x z
            m_Communicator.SubsribeEvent(E_Cha_MoveRequest_XZ.ID, AvoidIntersection_XZ, 0);

            //avoid intersection y
            m_Communicator.SubsribeEvent(E_Cha_MoveRequest_Y.ID, AvoidInterSection_Y, 2);

            //Handle jump request
            m_Communicator.SubsribeEvent(E_Cha_StartJump.ID, HandleJump, 2);



            m_World = Locator<IWorld>.GetService();
        }


        //intersection Handler
        //---------------------------------------------------------------------
        private bool HandleJump(IEvent _event)
        {
            E_Cha_StartJump EJump = _event as E_Cha_StartJump;
            m_Communicator.PublishEvent(new E_Cha_MoveRequest_Y(EJump.Force));
            return false;
        }


        private bool AvoidIntersection_XZ(IEvent _event)
        {
            E_Cha_MoveRequest_XZ req = (_event as E_Cha_MoveRequest_XZ);

            Vector3 Intersection;
            if (req.Translation.z > 0 && Front(req.Translation.z,out Intersection))
            {
                req.Translation -= Intersection;
            }

            else if(req.Translation.z < 0 && Back(req.Translation.z, out Intersection))
            {
                req.Translation += Intersection;
            }

            if (req.Translation.x > 0 && Right(req.Translation.x, out Intersection))
            {
                req.Translation -= Intersection;
            }
            else if (req.Translation.x < 0 && Left(req.Translation.x, out Intersection))
            {
                req.Translation += Intersection;
            }

            return true;           
        }

        //Jump request Handler
        private bool AvoidInterSection_Y(IEvent _event)
        {
            E_Cha_MoveRequest_Y Y = _event as E_Cha_MoveRequest_Y;

            if (Y.Speed > 0 && Up(Y.Speed, out Vector3 intersection))
            {
                Y.Speed -= intersection.y;

                m_Communicator.PublishEvent(new E_Cha_TouchUpsideBlock());
            }

            else if (Y.Speed < 0 && Down(Y.Speed, out intersection))
            {
                Y.Speed += intersection.y;
                m_Communicator.PublishEvent(new E_Cha_TouchGround());
                Debug.Log("tg");
            }

            return true;
        }

        //Check if there is a solid block in front of me
        private bool Front(float trans_z,out Vector3 intersect)
        {
            intersect = Vector3.zero;

            for (int i = 0; i < m_BodyHeight; ++i)
            {
                BlockLocation Loc = new BlockLocation(transform.position + new Vector3(0, i, m_BodyWidth_Half + trans_z), m_World);

                if (Loc.IsBlockExists() && Loc.CurBlockRef.IsObstacle)
                {
                    intersect.z = transform.position.z + trans_z + m_BodyWidth_Half- Loc.Bound.min.z;
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
                BlockLocation Loc = new BlockLocation(transform.position + new Vector3(0, i, -m_BodyWidth_Half + trans_z), m_World);

                if (Loc.IsBlockExists() && Loc.CurBlockRef.IsObstacle)
                {
                    intersect.z = Loc.Bound.max.z - (transform.position.z + trans_z - m_BodyWidth_Half);
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
                BlockLocation Loc = new BlockLocation(transform.position + new Vector3(-m_BodyWidth_Half + trans_x, i, 0), m_World);

                if (Loc.IsBlockExists() && Loc.CurBlockRef.IsObstacle)
                {
                    intersect.x = Loc.Bound.max.x-(transform.position.x + trans_x - m_BodyWidth_Half);
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
                    (transform.position + new Vector3(m_BodyWidth_Half + trans_x, i, 0), m_World);
                
                if (Loc.IsBlockExists() && Loc.CurBlockRef.IsObstacle)
                {
                    intersect.x = (transform.position.x + trans_x + m_BodyWidth_Half) - Loc.Bound.min.x;
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
                new BlockLocation(transform.position + new Vector3(0, trans_y, 0), m_World);

            if (Loc.IsBlockExists() && Loc.CurBlockRef.IsObstacle)
            {
                intersect.y = Loc.Bound.max.y - (transform.position.y + trans_y);
                //Debug.Log("Down intersection" + intersect);
                return true;
            }
            return false;
        }
        private bool Up(float trans_y, out Vector3 intersect)
        {
            intersect = Vector3.zero;
            BlockLocation Loc =
                new BlockLocation(transform.position + new Vector3(0, trans_y + m_BodyHeight, 0), m_World);

            if (Loc.IsBlockExists() && Loc.CurBlockRef.IsObstacle)
            {
                intersect.y = (transform.position.y + trans_y + m_BodyHeight) - Loc.Bound.min.y ;

                return true;
            }
            return false;
        }
    }

}

 


