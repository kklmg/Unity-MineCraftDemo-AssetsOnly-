using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Pattern;
using Assets.Scripts.World;

namespace Assets
{
    [RequireComponent(typeof(Player))]
    class WorldCollider : MonoBehaviour
    {
        [SerializeField]
        float m_Gravity = 1.5f;
        [SerializeField]
        int m_Height;



        private Player m_refPlayer;
        private World m_refWorld;
        //private MyObserver<Vector3Int> m_obsPlayerSlot;
        

        void Awake()
        {
            m_refPlayer = GetComponent<Player>();
            m_refWorld = m_refPlayer.m_RefWorld;

            //m_refPlayer.SubjectWorldSlot.Subscribe(m_obsPlayerSlot);
        }
        private void FixedUpdate()
        {
            if (Down())
            {
               // Debug.Log("at ground");
            }
            else transform.position = transform.position + Vector3.down * m_Gravity * Time.deltaTime;
            //m_obsPlayerSlot.Data
        }

        public bool IsObstacle(Vector3Int offset)
        {
            //Vector3Int Cur = new Vector3Int(
            //    (int)transform.position.x,
            //    (int)transform.position.y,
            //    (int)transform.position.z);

            //Vector3Int CheckPos = Cur + offset;
            //Block target;
            //for (int i = 0; i < m_Height; ++i)
            //{
            //    target = m_refWorld.GetBlock(CheckPos).IsSolid();
            
            //}

            //m_refPlayer.WorldSlot - 1;


            return true;

        }

        public bool Front()
        {
            return true;
        }
        public bool Back()
        {
            return true;
        }

        public bool Down()
        {
            Vector3Int CheckPos = new Vector3Int(
                (int)transform.position.x,
                (int)transform.position.y - 1,
                (int)transform.position.z);

            Block target = m_refWorld.GetBlock(CheckPos);

            if (target == null) return false;

            return target.IsSolid(eDirection.up);
        }
    }
}
