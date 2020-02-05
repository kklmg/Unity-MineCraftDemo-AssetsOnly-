using UnityEngine;
using Assets.Scripts.NGlobal.ServiceLocator;
using System.Collections;

namespace Assets.Scripts.NWorld
{
    public class Chunk : MonoBehaviour
    {
        //Field
        //------------------------------------------------------------------------
        public GameObject m_Prefab_Section;


        [SerializeField]
        ChunkInWorld m_ChunkinWorld;
        [SerializeField]
        private Vector2Int m_Coord;

        private IWorld m_refWorld;
        private Biome m_refBiome;

        [SerializeField]
        private ChunkHeightMap m_HeightMap;

        //[SerializeField]
        private Section[] m_arrSections;  //Sections

        public Section GetSection(SectionInChunk slot, bool CreateBlank = false)
        {
            if (slot.Value < 0 || slot.Value > m_refWorld.Chunk_Height) return null;

            if (m_arrSections[slot.Value] == null && CreateBlank)
            {
                CreateBlankSection(slot.Value);
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
        public bool GetGroundHeight(int blkx, int blkz, int CurY, out float GroundY)
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
                    CurY -= blky + 1;
                    continue;
                }

                while (blky > -1)
                {
                    CurBlock = CurSec.GetBlock(new BlockInSection(blkx, blky, blkz, m_refWorld));

                    if (CurBlock != null && CurBlock.IsSolid(Direction.UP))
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



        //Unity Funciton
        //------------------------------------------------------------------------------
        private void Awake()
        {
            m_refWorld = transform.parent.GetComponent<World>();
            m_arrSections = new Section[m_refWorld.Chunk_Height];

            m_refWorld = Locator<IWorld>.GetService();
            m_HeightMap = new ChunkHeightMap(m_refWorld);
        }

        public void Init(ChunkInWorld chunkInWorld)
        {
            //Save Position Data
            m_ChunkinWorld = chunkInWorld;
            m_Coord = chunkInWorld.ToCoord2DInt(m_refWorld);

            //Get Biome Data
            m_refBiome = m_refWorld.GetBiome(m_ChunkinWorld);

            //create Height map
            m_HeightMap.Generate(m_Coord,m_refBiome,m_refWorld);

            //Create Sections
            CreateAllSections();
            //StartCoroutine("CreCreateAllSections_Corou");
        }

        private void CreateBlankSection(int slot_y)
        {
            Debug.Assert(m_arrSections[slot_y] == null);

            //make a Section instance
            m_arrSections[slot_y] =
                  Instantiate(m_Prefab_Section,
                  new Vector3(m_Coord.x, slot_y * m_refWorld.Chunk_Height, m_Coord.y),
                  Quaternion.identity, transform).GetComponent<Section>();

            //set name
            m_arrSections[slot_y].transform.name = "Section" + '[' + slot_y + ']';

            //set Section
            m_arrSections[slot_y].SectionInWorld =
                new SectionInWorld(m_ChunkinWorld.Value.x, slot_y, m_ChunkinWorld.Value.y);
            m_arrSections[slot_y].GenerateBlankSection();
        }

        //create instance of Section
        private void CreateSection(int slot_y)
        {
            int sec_minHeight = slot_y * m_refWorld.Chunk_Height;

            //case1: section lowest height > terrain tallest height
            //case2: section tallest height < terrain lowetst height
            if (sec_minHeight > m_HeightMap.MaxHeight 
                || sec_minHeight + m_refWorld.Chunk_Height < m_HeightMap.MinHeight)
            {
                if (m_arrSections[slot_y] != null)
                {
                    m_arrSections[slot_y].gameObject.SetActive(false);
                    Destroy(m_arrSections[slot_y].gameObject);
                    m_arrSections[slot_y] = null;
                }
                return;
            }

            if (m_arrSections[slot_y] == null)
            {
                //make a Section instance
                m_arrSections[slot_y] =
                    Instantiate(m_Prefab_Section, transform).GetComponent<Section>();
            }

            //Clear Mesh
            m_arrSections[slot_y].ClearMesh();

            //set position
            m_arrSections[slot_y].transform.localPosition = new Vector3(0, slot_y * m_refWorld.Chunk_Height, 0);

            //set name
            m_arrSections[slot_y].transform.name = "Section" + '[' + slot_y + ']';

            //set Section
            m_arrSections[slot_y].SectionInWorld =
                new SectionInWorld(m_ChunkinWorld.Value.x, slot_y, m_ChunkinWorld.Value.y);

            //Init blocks in Section
            m_arrSections[slot_y].GenerateSection_ByLayer(m_refBiome.Layer, m_HeightMap, slot_y * m_refWorld.Chunk_Height);
        }

        private void CreateAllSections()
        {
            for (int i = 0; i < m_refWorld.Chunk_Height; ++i)
            {
                CreateSection(i);
            }
        }
    }
}


