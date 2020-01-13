using System;
using Assets.Scripts.Pattern;
using Assets.Scripts.NWorld;
using Assets.Scripts.EventManager;
using UnityEngine;

namespace Assets.Scripts.CharacterSpace
{
    [RequireComponent(typeof(Communicator))]
    class ChaCollider : MonoBehaviour
    {
        private World m_refWorld;
        private Character m_refCha;

        //Temp Cache
        private ChaMove m_ChaMove;
        //private Vector3Int
        public Bounds px;
        public Bounds nx;
        public Bounds m_BodyBound;

        void Start()
        {
            GetComponent<Communicator>().SubsribeEvent(E_Cha_TryMove.ID, Handle);
            m_refCha = GetComponent<Character>();
            m_refWorld = Locator<World>.GetService(); 
        }
        private void Update()
        {
            m_BodyBound.center = transform.position;
        }
        private bool Handle(IEvent _event)
        {
            m_ChaMove = (_event as E_Cha_TryMove).Move_Data;

            //temp
            Vector3 Trans = m_ChaMove.Translation;
            float magnitude = Trans.magnitude;
            Vector3 Normal = Trans.normalized;
            
            Block adjBlk;

            Bounds CurBound = m_refWorld.GetBound(transform.position);

            int x = Math.Abs((int)Trans.x);
            int y = Math.Abs((int)Trans.y);
            int z = Math.Abs((int)Trans.z);

            while (x!=0 || y!=0 || z!=0)
            {



            }



            if (!m_BodyBound.Intersects(CurBound))
            {
            }


            if (m_BodyBound.min.x + Trans.x > m_BodyBound.min.x)
            {
                adjBlk = m_refWorld.GetBlock(transform.position + Vector3.right);

                //collide with block
                if (adjBlk != null && adjBlk.IsSolid(Direction.LEFT))
                {
                    px = m_refWorld.GetBound(transform.position + Vector3.right);
                    //Debug.Log("collision+x");
                    m_ChaMove.Translation.x = 0;
                    //m_ChaMove.Translation.x = px.min.x - transform.position.x;
                }
            }

            if (Trans.x - m_refCha.BodyRadius < 0)
            {
                adjBlk = m_refWorld.GetBlock(transform.position + Vector3.left);

                //collide with block
                if (adjBlk != null && adjBlk.IsSolid(Direction.RIGHT))
                {
                    nx = m_refWorld.GetBound(transform.position + Vector3.left);
                    Debug.Log("collision-x");
                    //limit Translation
                    m_ChaMove.Translation.x = 0;
                    //m_ChaMove.Translation.x = nx.max.x - transform.position.x;
                }
            }




            ////right
            //while (magnitude > 1)
            //{



            //    Trans -= 



            //}


            //if (m_ChaMove.Translation.x > 0)
            //{
            //    adj = world.GetBlock(transform.position + new Vector3(HalfWidth + 1, 0,0));
            //    if (adj != null && adj.IsSolid(eDirection.left))
            //    {
            //        ThisData.Translation.x = 0;
            //    }
            //}
            ////left
            //else
            //{
            //    adj = world.GetBlock(transform.position + new Vector3(-HalfWidth - 1, 0, 0));
            //    if (adj != null && adj.IsSolid(eDirection.left))
            //    {
            //        //ThisData.Trans_x = Mathf.Min(ThisData.Trans_x, 1 - (transform.position.x % 1));
            //        ThisData.Trans_x = 0;
            //    }
            //}

            ////forward
            //if (ThisData.Trans_z > 0)
            //{
            //    adj = world.GetBlock(transform.position + new Vector3(0, 0, HalfWidth + 1));
            //    if (adj != null && adj.IsSolid(eDirection.backward))
            //    {
            //        ThisData.Trans_z = 0;
            //        //movement.z = Mathf.Min(movement.z, 1 - (trans.position.z % 1));
            //    }
            //}
            ////backward
            //else
            //{
            //    adj = world.GetBlock(transform.position + new Vector3(0, 0, -HalfWidth - 1));
            //    if (adj != null && adj.IsSolid(eDirection.forward))
            //    {
            //        ThisData.Trans_z = 0;
            //        //movement.z = Mathf.Min(movement.z, 1 - (trans.position.z % 1));
            //    }
            //}


            ////up
            //if (movement.y > 0)
            //{
            //    adj = world.GetBlock(trans.position + Vector3.up);
            //    if (adj != null && adj.IsSolid(eDirection.down))
            //    {
            //        movement.y = Mathf.Min(movement.y, 1 - (trans.position.y % 1));
            //        Debug.Log(movement);
            //    }
            //}
            ////down
            //else
            //{
            //    adj = world.GetBlock(trans.position + Vector3.down);
            //    if (adj != null && adj.IsSolid(eDirection.up))
            //    {
            //        movement.y = Mathf.Min(movement.y, 1 - (trans.position.y % 1));
            //        Debug.Log(movement);
            //    }
            //}
            return true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawCube(m_BodyBound.center, Vector3.one);
            Gizmos.DrawCube(px.center, Vector3.one);
            Gizmos.DrawCube(nx.center, Vector3.one);
        }

    }

}

 


