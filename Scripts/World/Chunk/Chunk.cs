using UnityEngine;

namespace Assets.Scripts.WorldComponent
{
    public class Chunk : MonoBehaviour
    {
        //Field
        //------------------------------------------------------------------------
        [SerializeField]
        private int m_WorldSlot_x;
        [SerializeField]
        private int m_WorldSlot_z;
        [SerializeField]
        private int m_Coord_x;
        [SerializeField]
        private int m_Coord_z;

        private int m_MaxHeight;
        private int m_MinHeight;
        private Transform m_Parent;
        private World m_refWorld;
        private Biome m_refBiome;

        [SerializeField]
        int[,] m_arrHeightMap;

        //[SerializeField]
        private Section[] m_arrSections;  //Sections

        public Section GetSection(int h)
        {
            h = h % m_refWorld.Chunk_Height;
            
            if (h < 0 || h > m_refWorld.Chunk_Height) return null;
            return m_arrSections[h];
        }
        //public float GetGroundHeight(int x,int y,int z)
        //{
        //    while (y>-1)
        //    {
        //        int Height = y % m_refWorld.Chunk_Height;
        //        int SHeight = y % m_refWorld.Section_Height;
        //        Section cur = GetSection(Height);
        //        if (cur == null)
        //        {
        //            y -= m_refWorld.Chunk_Height;
        //            continue;
        //        }

        //        if (SHeight>-1)
        //        {
        //            block.x;

        //            if(cur.GetBlock().IsSolid(eDirection.up))

        //        }
        //        m_arrSections[]
        //    }
        //    return m_arrHeightMap[x, z];
        //}

        //public float GetBlock(int y)
        //{
        //    return m_arrHeightMap[x, z];
        //}


        //property
        //------------------------------------------------------------------------

        //public Vector2Int WorldPos { get { return m_PosWorld; } set { m_PosWorld = value; } }
        public World WorldReference { get { return m_refWorld; } }


        //Unity Funciton
        //------------------------------------------------------------------------------
        private void Awake()
        {
            m_refWorld = transform.parent.GetComponent<World>();
            m_arrSections = new Section[m_refWorld.Chunk_Height];
        }

        public void Init(int slot_x, int slot_z, Transform parent, Biome RefBiome)
        {
            m_WorldSlot_x = slot_x;
            m_WorldSlot_z = slot_z;

            m_Coord_x = slot_x * m_refWorld.Section_Width;
            m_Coord_z = slot_z * m_refWorld.Section_Depth;

            m_Parent = parent;
            m_refBiome = RefBiome;

            m_arrHeightMap = m_refBiome.GenerateHeightMap
                (m_Coord_x, m_Coord_z, m_refWorld.Section_Width, m_refWorld.Section_Height,
                out m_MaxHeight,out m_MinHeight);

            CreateAllSections();
        }

        //create instance of Section
        public void CreateSection(int slot_y)
        {
            int sec_minHeight = slot_y * m_refWorld.Chunk_Height;

            if (sec_minHeight > m_MaxHeight) return;
            if (sec_minHeight + m_refWorld.Chunk_Height < m_MinHeight) return;

            GameObject NewGo = new GameObject("Section" + '[' + slot_y + ']');
            NewGo.transform.parent = transform;
            NewGo.transform.position = new Vector3(m_Coord_x, slot_y * m_refWorld.Chunk_Height, m_Coord_z);

            m_arrSections[slot_y] = NewGo.AddComponent<Section>();
            m_arrSections[slot_y].SectionSlot = new Vector3Int(m_WorldSlot_x, slot_y, m_WorldSlot_z);
            m_arrSections[slot_y].GenerateSection(m_refBiome.getLayerData(), m_arrHeightMap, slot_y * m_refWorld.Chunk_Height);
        }

        public void CreateAllSections()
        {
            for (int i = 0; i < m_refWorld.Chunk_Height; ++i)
            {
                CreateSection(i);
            }
            Debug.Log("section created");
        }
    }
}

