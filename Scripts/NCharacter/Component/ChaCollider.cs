using Assets.Scripts.NGlobal.ServiceLocator;

using Assets.Scripts.NWorld;
using Assets.Scripts.NEvent;

using UnityEngine;

namespace Assets.Scripts.NCharacter
{
    [RequireComponent(typeof(Communicator))]
    class ChaCollider : MonoBehaviour
    {
        private IWorld m_refWorld;

        //private Vector3Int
        [Range(0.1f, 0.9f)]
        public float m_BodyWidth = 0.8f;
        [Range(1, 3)]
        public float m_BodyHeight = 1.8f;

        private float m_BodyWidthExtent;
        private float m_BodyHeightExtent;


        //Unity Function
        //---------------------------------------------------------------------
        private void Start()
        {
            GetComponent<Communicator>().SubsribeEvent_Decorate(E_Cha_MoveRequest.ID, AvoidIntersect);
            m_refWorld = Locator<IWorld>.GetService();

            //Init Body Bound Data
            m_BodyWidthExtent = m_BodyWidth / 2;
            m_BodyHeightExtent = m_BodyHeight / 2;
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

        //Check if there is a solid block in front of me
        private bool Front(float trans_z,out Vector3 intersect)
        {
            intersect = Vector3.zero;

            for (int i = 0; i < m_BodyHeight; ++i)
            {
                BlockLocation Loc = new BlockLocation(transform.position + new Vector3(0, i, m_BodyWidthExtent + trans_z), m_refWorld);

                if (Loc.IsBlockExists() && Loc.CurBlockRef.IsSolid(Direction.BACKWARD))
                {
                    intersect.z = transform.position.z + trans_z + m_BodyWidthExtent- Loc.Bound.min.z;
                    Debug.Log("front intersection"+ intersect);
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

                if (Loc.IsBlockExists() && Loc.CurBlockRef.IsSolid(Direction.FORWARD))
                {
                    intersect.z = Loc.Bound.max.z - (transform.position.z + trans_z - m_BodyWidthExtent);
                    Debug.Log("back intersection" + intersect);
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

                if (Loc.IsBlockExists() && Loc.CurBlockRef.IsSolid(Direction.RIGHT))
                {
                    intersect.x = Loc.Bound.max.x-(transform.position.x + trans_x - m_BodyWidthExtent);
                    Debug.Log("left intersection" + intersect);
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
                
                if (Loc.IsBlockExists() && Loc.CurBlockRef.IsSolid(Direction.LEFT))
                {
                    intersect.x = (transform.position.x + trans_x + m_BodyWidthExtent) - Loc.Bound.min.x;
                    Debug.Log("right intersection" + intersect);
                    return true;
                }
            }
            return false;
        }

        //private bool Down(float trans_y, out Vector3 intersect)
        //{

        //}
        //private bool Up(float trans_y, out Vector3 intersect)
        //{
        //}
    }

}

 


