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

        public Section GetSection(int Height)
        {
            Height = Height % m_refWorld.Chunk_Height;
            
            if (Height < 0 || Height > m_refWorld.Chunk_Height) return null;
            return m_arrSections[Height];
        }
        public bool GetGroundHeight(int blkx,int blkz, int CurY,out float GroundY)
        {
            CurY -= 1;

            int SecID;
            int blky;
            Section CurSec;
            Block CurBlock;

            while (CurY > -1)
            {
                //Get Currect Section
                SecID = CurY / m_refWorld.Chunk_Height;
                CurSec = m_arrSections[SecID];
             
                //Block Height
                blky = CurY % m_refWorld.Chunk_Height;

                if (CurSec == null)
                {
                    CurY -= blky+1;
                    continue;
                }

                while (blky > -1)
                {
                    CurBlock = CurSec.GetBlock(blkx, blky, blkz);

                    if (CurBlock!=null && CurBlock.IsSolid(eDirection.up))
                    {
                        GroundY = (blky + 1) + SecID * m_refWorld.Chunk_Height;
                        return true;
                    }
                    blky -= 1;
                    CurY -= 1;
                }
            }
            GroundY = default(float);
            return false;
        }

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

            //create Height map
            m_arrHeightMap = m_refBiome.GenerateHeightMap
                (m_Coord_x, m_Coord_z, m_refWorld.Section_Width, m_refWorld.Section_Height,
                m_refWorld.NoiseMaker,out m_MaxHeight,out m_MinHeight);

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

