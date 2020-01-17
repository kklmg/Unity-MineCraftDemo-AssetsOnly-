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
        [Range(0.1f,0.5f)]
        public float m_BodyWidth;
        [Range(1,5)]
        public int m_BodyHeight;
        public Bounds _front;
        public Bounds _Back;
        public Bounds _left;
        public Bounds _right;
        public Bounds m_BodyBound;

        public Transform f, b, l, r,s;

        private Block m_CacheBlock;

        private void Start()
        {
            GetComponent<Communicator>().SubsribeEvent(E_Cha_TryMove.ID, Handle);
            m_refCha = GetComponent<Character>();
            m_refWorld = Locator<World>.GetService();

            m_BodyBound.center = transform.position;
        }

        private void Update()
        {
            _Back = m_refWorld.GetBound(transform.position + new Vector3(0, 0, -m_BodyWidth));
            _front = m_refWorld.GetBound(transform.position + new Vector3(0, 0, m_BodyWidth));
            _right = m_refWorld.GetBound(transform.position + new Vector3(m_BodyWidth, 0, 0));
            _left = m_refWorld.GetBound(transform.position + new Vector3(-m_BodyWidth, 0, 0));
            m_BodyBound = m_refWorld.GetBound(transform.position);

            //f.position = _front.center;
            //b.position = _Back.center;
            //r.position = _right.center;
            //l.position = _left.center;
           // s.position = transform.position ;

        }

        private bool Handle(IEvent _event)
        {
            m_ChaMove = (_event as E_Cha_TryMove).Move_Data;

            Vector3 realTrans = transform.rotation * m_ChaMove.Translation;

            if (realTrans.z > 0 && Front(realTrans.z))
            {
                m_ChaMove.Translation.z = 0;
            }
            else if (realTrans.z < 0 && Back(realTrans.z))
            {
                m_ChaMove.Translation.z = 0;
            }

            if (realTrans.x > 0 && Right(realTrans.x))
            {
                m_ChaMove.Translation.x = 0;
            }
            else if (realTrans.x < 0 && Left(realTrans.x))
            {
                m_ChaMove.Translation.x = 0;
            }
            ////update body bound
            //m_BodyBound.center = transform.position;

            ////temp
            //Vector3 Trans = m_ChaMove.Translation;
            //float Magnitude = Trans.magnitude;
            //Vector3 Normal = Trans.normalized;
            //Debug.Log(Trans);

            //Block adjBlk;

            //Bounds CurBound = m_refWorld.GetBound(transform.position);


            //if (transform.position.x + Trans.x)

            //Bounds X = m_refWorld.GetBound(transform.position+new Vector3);


            //if (!m_BodyBound.Intersects(CurBound))
            //{
            //}


            //if (m_BodyBound.min.x + Trans.x > m_BodyBound.min.x)
            //{
            //    adjBlk = m_refWorld.GetBlock(transform.position + Vector3.right);

            //    //collide with block
            //    if (adjBlk != null && adjBlk.IsSolid(Direction.LEFT))
            //    {
            //        px = m_refWorld.GetBound(transform.position + Vector3.right);
            //        //Debug.Log("collision+x");
            //        m_ChaMove.Translation.x = 0;
            //        //m_ChaMove.Translation.x = px.min.x - transform.position.x;
            //    }
            //}

            //if (Trans.x - m_refCha.BodyRadius < 0)
            //{
            //    adjBlk = m_refWorld.GetBlock(transform.position + Vector3.left);

            //    //collide with block
            //    if (adjBlk != null && adjBlk.IsSolid(Direction.RIGHT))
            //    {
            //        nx = m_refWorld.GetBound(transform.position + Vector3.left);
            //        Debug.Log("collision-x");

            //        //limit Translation
            //        m_ChaMove.Translation.x = 0;
            //        //m_ChaMove.Translation.x = nx.max.x - transform.position.x;
            //    }
            //}




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


        private bool Front(float trans_z)
        {
            for (int i = 0; i < m_BodyHeight; ++i)
            {
                m_CacheBlock = m_refWorld.GetBlock(transform.position + new Vector3(0, i, m_BodyWidth + trans_z));
                if (m_CacheBlock != null && m_CacheBlock.IsSolid(Direction.BACKWARD)) return true;
            }
            return false;
        }
        private bool Back(float trans_z)
        {
            for (int i = 0; i < m_BodyHeight; ++i)
            {
                m_CacheBlock = m_refWorld.GetBlock(transform.position + new Vector3(0, i, -m_BodyWidth+ trans_z));
                if (m_CacheBlock != null && m_CacheBlock.IsSolid(Direction.FORWARD)) return true;
            }
            return false;
        }
        private bool Left(float trans_x)
        {
            for (int i = 0; i < m_BodyHeight; ++i)
            {
                m_CacheBlock = m_refWorld.GetBlock(transform.position + new Vector3(-m_BodyWidth + trans_x, i,0));
                if (m_CacheBlock != null && m_CacheBlock.IsSolid(Direction.RIGHT)) return true;
            }
            return false;
        }
        private bool Right(float trans_x)
        {
            for (int i = 0; i < m_BodyHeight; ++i)
            {
                m_CacheBlock = m_refWorld.GetBlock(transform.position + new Vector3(m_BodyWidth + trans_x, i,0));
                if (m_CacheBlock != null && m_CacheBlock.IsSolid(Direction.LEFT)) return true;
            }
            return false;
        }
    }

}

 


