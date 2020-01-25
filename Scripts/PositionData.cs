using UnityEngine;
using Assets.Scripts.NWorld;
using Assets.Scripts.NGlobal.ServiceLocator;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Character))]
    class PositionData : MonoBehaviour
    {
        //Field
        //-------------------------------------------------------------------------------      
        public IWorld m_refWorld = null;
        public Vector3Int m_SecSlot;
        public Chunk m_Chunk = null;
        public Section m_Section = null;
        public Vector3Int m_BlkSlot;
        public Block m_Block = null;
        public Bounds m_bound;

        //property
        //-------------------------------------------------------------------------------
        public Section CurSection { get { return m_Section; } }
        [SerializeField]
        public Vector3Int SectionSlot { get { return m_SecSlot; } }
        [SerializeField]
        public Vector3Int BlockSlot { get { return m_BlkSlot; } }

        private void Awake()
        {        
            m_refWorld = Locator<IWorld>.GetService();
        }

        //private void FixedUpdate()
        //{
        //    m_bound = m_refWorld.GetBound(transform.position);

        //    //get section position in world
        //    m_SecSlot = m_refWorld.CoordToSlot(transform.position);
        //    m_BlkSlot = new Vector3Int(
        //        (int)transform.position.x % m_refWorld.Section_Width,
        //        (int)transform.position.y % m_refWorld.Section_Height,
        //        (int)transform.position.z % m_refWorld.Section_Depth);

        //    //get current chunk
        //    m_Chunk = m_refWorld.GetChunk(m_SecSlot);
        //    if (m_Chunk == null)
        //    {
        //        m_Section = null;
        //        m_Block = null;
        //        return;
        //    }

        //    //update section
        //    m_Section = m_Chunk.GetSection(m_SecSlot.y);
        //    if (m_Section == null)
        //    {
        //        m_Block = null;
        //        return;
        //    }
        //    //update block
        //    m_Section.GetBlock(m_BlkSlot.x, m_BlkSlot.y, m_BlkSlot.z);


            
        //}
        //void OnGUI()
        //{
        //    GUI.Label(new Rect(0 + 10, 0 + 10, 100, 20), transform.position.ToString());
        //    GUI.Label(new Rect(0 + 10, 0 + 30, 100, 20), m_bound.min.ToString());
        //    GUI.Label(new Rect(0 + 10, 0 + 50, 100, 20), m_bound.max.ToString());
        //}
        //private void OnDrawGizmos()
        //{
        //    Gizmos.DrawSphere(transform.position, 0.2f);
        //    //Gizmos.DrawCube(m_bound.center, new Vector3(1,1,1));           
        //}
    }

}
