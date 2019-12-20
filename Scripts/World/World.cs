using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Pattern;

namespace Assets.Scripts.World
{
    public class World : MonoBehaviour
    {
        //Filed-----------------------------------------------

        //Section Size 
        [SerializeField]
        private ushort m_Section_width = 16;
        [SerializeField]
        private ushort m_Section_height = 16;
        [SerializeField]
        private ushort m_Section_depth = 16;

        //chunk count 
        [SerializeField]
        private ushort m_count_x = 2;
        [SerializeField]
        private ushort m_count_y = 1;
        [SerializeField]
        private ushort m_count_z = 1;

        [SerializeField]
        private ChunkPool m_ChunkPool;


        //terrain size
        private uint m_TotalWidth;
        private uint m_TotalDepth;
        private uint m_TotalHeight;

        //spawn position
        public int x;
        public int z;

        //Biomes
        [SerializeField]
        private List<Biome> m_Bimoes;

        //[SerializeField]
        private Dictionary<Vector3Int, Section> m_SectionMap;

        [SerializeField]
        private List<Block> m_listBlocks;

        [SerializeField]
        private TextureSheet m_TextureSheet;

        public Transform player;

        //Property-------------------------------------------------------
        public ushort C_WIDTH { get { return m_Section_width; } }
        public ushort C_HEIGHT { get { return m_Section_height; } }
        public ushort C_DEPTH { get { return m_Section_depth; } }

        public uint TOTAL_WIDTH { get { return m_TotalWidth; } }
        public uint TOTAL_DEPTH { get { return m_TotalDepth; } }
        public uint TOTAL_HEIGHT { get { return m_TotalHeight; } }

        public List<Block> BlockList { get { return m_listBlocks; } }
        public TextureSheet TexSheet { get { return m_TextureSheet; } }

        public Vector3Int CoordToSlot(Vector3 pos)
        {
            return new Vector3Int((int)pos.x / m_Section_width, (int)pos.y / m_Section_height, (int)pos.z / m_Section_depth);
        }

        //unity function------------------------------------------------
        private void Awake()
        {
            m_TotalWidth = (uint)(m_Section_width * m_count_x);
            m_TotalHeight = (uint)(m_Section_height * m_count_y);
            m_TotalDepth = (uint)(m_Section_depth * m_count_z);
            //Debug.Log("world start");

            //testing
            m_SectionMap = new Dictionary<Vector3Int, Section>();
            m_ChunkPool = new ChunkPool(9);
        }
        

        //Function---------------------------------------------------
        public void CreateChunk(int x, int z)
        {
            m_ChunkPool.Spawn(x, z, this, this.transform, m_Bimoes[0]);
            Debug.Log("chunk Created");
        }

        public void RegisterSection(Vector3Int slot,Section _section)
        {
            m_SectionMap.Add(slot, _section);
        }
       
        public Section GetSection(int x,int y,int z)
        {
            //if (x > -1 && x < m_count_x && y > -1 && y < m_count_y && z > -1 && z < m_count_z)
            //{
            //    return m_arrChunks[x, y, z];
            //}
            //else return null;
            return null;
        }

        public Section GetSection(Vector3Int Slot)
        {
            Section receiver;
            if (m_SectionMap.TryGetValue(Slot, out receiver))
            {
                return receiver;
            }
            else return null;
        }
    }
}
