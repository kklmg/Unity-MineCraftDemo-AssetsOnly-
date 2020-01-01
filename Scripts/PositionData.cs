using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.WorldComponent;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Character))]
    class PositionData : MonoBehaviour
    {
        //Field
        //-------------------------------------------------------------------------------
        //cache components
        private World m_refWorld;
        private Transform m_TransSelf;

        [SerializeField]
        private Vector3Int m_WorldPos;
        [SerializeField]
        private Section m_CurSection;
        [SerializeField]
        private Vector3Int m_CurSectionSlot;
        [SerializeField]
        private Vector3Int m_CurBlockSlot;

        //property
        //-------------------------------------------------------------------------------
        public Section CurSection { get { return m_CurSection; } }
        [SerializeField]
        public Vector3Int CurSectionSlot { get { return m_CurSectionSlot; } }
        [SerializeField]
        public Vector3Int CurBlockSlot { get { return m_CurBlockSlot; } }

        private void Awake()
        {
            m_TransSelf = this.transform;
            m_refWorld = GetComponent<Character>().refWorld;
        }
        private void FixedUpdate()
        {
            m_WorldPos = new Vector3Int(
                (int)m_TransSelf.position.x, 
                (int)m_TransSelf.position.y, 
                (int)m_TransSelf.position.z );

            m_CurSectionSlot = m_refWorld.CoordToSectionSlot(m_WorldPos);
            m_CurSection = m_refWorld.GetSection(m_CurSectionSlot);

            m_CurBlockSlot = new Vector3Int(
                m_WorldPos.x % m_refWorld.Section_Width,
                m_WorldPos.y % m_refWorld.Section_Height,
                m_WorldPos.z % m_refWorld.Section_Depth);
        }

    }
}
