using UnityEngine;

namespace Assets.Scripts.NWorld
{
    public class Chunk : MonoBehaviour
    {
        //Field
        //------------------------------------------------------------------------
        [SerializeField]
        ChunkInWorld m_ChunkinWorld;
        [SerializeField]
        private Vector2Int m_Coord;

        private int m_MaxHeight;
        private int m_MinHeight;
        private Transform m_Parent;
        private World m_refWorld;
        private Biome m_refBiome;

        [SerializeField]
        int[,] m_arrHeightMap;

        //[SerializeField]
        private Section[] m_arrSections;  //Sections
        public Section GetSection(SectionInChunk slot,bool CreateBlank = false)
        {  
            if (slot.Value < 0 || slot.Value > m_refWorld.Chunk_Height) return null;

            if (m_arrSections[slot.Value] == null && CreateBlank)
            {
                CreateNewSection(slot.Value);
            }
            return m_arrSections[slot.Value];
        }
        public bool TryGetSection(SectionInChunk slot, out Section Sec)
        {
            Sec = default(Section);

            slot.Value = slot.Value % m_refWorld.Chunk_Height;

            if (slot.Value < 0 || slot.Value > m_refWorld.Chunk_Height)
            {
                return false;
            }
            else
            {
                Sec = m_arrSections[slot.Value];
                return true;
            }
               
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

                    if (CurBlock!=null && CurBlock.IsSolid(Direction.UP))
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
        public World WorldReference { get { return m_refWorld; } }


        //Unity Funciton
        //------------------------------------------------------------------------------
        private void Awake()
        {
            m_refWorld = transform.parent.GetComponent<World>();
            m_arrSections = new Section[m_refWorld.Chunk_Height];
        }

        public void Init(ChunkInWorld chunkpos, Transform parent, Biome refBiome,IWorld _world)
        {
            m_ChunkinWorld = chunkpos;
            m_Coord = chunkpos.ToCoord2DInt(_world);

            m_Parent = parent;
            m_refBiome = refBiome;

            //create Height map
            m_arrHeightMap = m_refBiome.GenerateHeightMap
                (m_ChunkinWorld.Value, m_refWorld.Section_Width, m_refWorld.Section_Height,
                m_refWorld.NoiseMaker,out m_MaxHeight,out m_MinHeight);

            //Create Sections
            CreateAllSections();
        }

        private void CreateNewSection(int slot_y)
        {
            Debug.Assert(m_arrSections[slot_y] == null);

            GameObject NewGo = new GameObject("Section" + '[' + slot_y + ']');
            NewGo.transform.parent = transform;
            NewGo.transform.position = new Vector3(m_Coord.x, slot_y * m_refWorld.Chunk_Height, m_Coord.y);

            m_arrSections[slot_y] = NewGo.AddComponent<Section>();
            m_arrSections[slot_y].SectionSlot =
                new SectionInWorld(m_ChunkinWorld.Value.x, slot_y, m_ChunkinWorld.Value.y);
            m_arrSections[slot_y].GenerateBlankSection();
        }
        //create instance of Section
        private void CreateSection(int slot_y)
        {
            int sec_minHeight = slot_y * m_refWorld.Chunk_Height;

            //case1: section lowest height > terrain tallest height
            //case2: section tallest height < terrain lowetst height
            if (sec_minHeight > m_MaxHeight || sec_minHeight + m_refWorld.Chunk_Height < m_MinHeight)
            {
                return;
            } 
            

            GameObject NewGo = new GameObject("Section" + '[' + slot_y + ']');
            NewGo.transform.parent = transform;
            NewGo.transform.position = new Vector3(m_Coord.x, slot_y * m_refWorld.Chunk_Height, m_Coord.y);

            m_arrSections[slot_y] = NewGo.AddComponent<Section>();
            m_arrSections[slot_y].SectionSlot =
                new SectionInWorld(m_ChunkinWorld.Value.x, slot_y, m_ChunkinWorld.Value.y);

            //Init blocks in Section
            m_arrSections[slot_y].GenerateSection_ByLayer(m_refBiome.getLayerData(), m_arrHeightMap, slot_y * m_refWorld.Chunk_Height);
        }
        private void CreateAllSections()
        {
            for (int i = 0; i < m_refWorld.Chunk_Height; ++i)
            {
                CreateSection(i);
            }
            Debug.Log("section created");
        }
    }
}

