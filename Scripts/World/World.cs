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

        //Chunk Size 
        [SerializeField]
        private ushort m_chunk_width = 16;
        [SerializeField]
        private ushort m_chunk_height = 16;
        [SerializeField]
        private ushort m_chunk_depth = 16;

        //chunk count 
        [SerializeField]
        private ushort m_count_x = 2;
        [SerializeField]
        private ushort m_count_y = 1;
        [SerializeField]
        private ushort m_count_z = 1;

        [SerializeField]
        private ChunkColPool m_ChunkColPool;


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
        private Dictionary<Vector3Int, Chunk> m_ChunksMap;

        [SerializeField]
        private List<Block> m_listBlocks;
        //[SerializeField]
        //private Chunk[,,] m_arrChunks;
        [SerializeField]
        private TextureSheet m_TextureSheet;

        //Property-------------------------------------------------------
        public ushort C_WIDTH { get { return m_chunk_width; } }
        public ushort C_HEIGHT { get { return m_chunk_height; } }
        public ushort C_DEPTH { get { return m_chunk_depth; } }

        public uint TOTAL_WIDTH { get { return m_TotalWidth; } }
        public uint TOTAL_DEPTH { get { return m_TotalDepth; } }
        public uint TOTAL_HEIGHT { get { return m_TotalHeight; } }

        public List<Block> BlockList { get { return m_listBlocks; } }
        public TextureSheet TexSheet { get { return m_TextureSheet; } }

        //unity function------------------------------------------------
        private void Start()
        {
            m_TotalWidth = (uint)(m_chunk_width * m_count_x);
            m_TotalHeight = (uint)(m_chunk_height * m_count_y);
            m_TotalDepth = (uint)(m_chunk_depth * m_count_z);
            Debug.Log("world start");
            //testing
            m_ChunksMap = new Dictionary<Vector3Int, Chunk>();
            m_ChunkColPool = new ChunkColPool(5);

            m_ChunkColPool.CreateChunkCol(x, z, this, this.transform, m_Bimoes[0]);
            //m_ChunkColPool.CreateChunkCol(x + 1, z + 1, this, this.transform, m_Bimoes[0]);
            m_ChunkColPool.CreateChunkCol(x, z + 1, this, this.transform, m_Bimoes[0]);
            //m_ChunkColPool.CreateChunkCol(x + 1, z, this, this.transform, m_Bimoes[0]);
            //m_ChunkColPool.CreateChunkCol(x + 2, z, this, this.transform, m_Bimoes[0]);

            Debug.Log("chunk created");

            //ChunkColumn ck = new ChunkColumn(x, z, this, this.transform,m_Bimoes[0]);
            //ChunkColumn cck = new ChunkColumn(x+ m_chunk_width, z+m_chunk_depth, this, this.transform, m_Bimoes[0]);
            //ChunkColumn ccck = new ChunkColumn(x, z + m_chunk_depth, this, this.transform, m_Bimoes[0]);
            //ChunkColumn cccck = new ChunkColumn(x + m_chunk_width, z, this, this.transform, m_Bimoes[0]);
            //InitWorld();
        }
        

        //Function---------------------------------------------------
        void InitWorld()
        {
            GameObject go;
            int i, j, k;
            for (i = 0; i < m_count_x; ++i)
            {
                for (j = 0; j < m_count_y; ++j)
                {
                    for (k = 0; k < m_count_z; ++k)
                    {
                        //Instantiate<GameObject>()
                        go = new GameObject("Chunk" + '[' + i + ']' + '[' + j + ']' + '[' + k + ']');
                     
                        //set position
                        go.transform.position = new Vector3(i * m_chunk_width, j * m_chunk_height, k * m_chunk_depth);
                        go.transform.parent = this.transform;

                        go.AddComponent<Chunk>();
                        //save chunk reference
                        Chunk refChunk = go.GetComponent<Chunk>();
                        //m_arrChunks[i, j, k] = refChunk;

                        //set relative position
                        //refChunk.WorldPos = new Vector3Int(i, j, k);


                    }
                }
            }
        }

        void CreateChunkColumn(int x, int z)
        {
            ChunkColumn ck = new ChunkColumn( x, z, this, this.transform,m_Bimoes[0]);

            
            //Instantiate<GameObject>()
            //GameObject go = new GameObject("Chunk" + '[' + x + ']' + '[' + z+ ']');

            //set position
            //go.transform.position = new Vector3(x * m_chunk_width,z * m_chunk_depth);
            //go.transform.parent = this.transform;

            // m_ChunkColumnPool.

            //ChunkColumn refChunk = go.AddComponent<ChunkColumn>();

            //save chunk reference
            //m_arrChunks[i, j, k] = refChunk;

            //set relative position
            //refChunk.WorldPos = new Vector2Int(x,z);

        }

        public void RegisterChunk(Vector3Int slot,Chunk _chunk)
        {
            m_ChunksMap.Add(slot, _chunk);
        }
       
        public Chunk GetChunk(int x,int y,int z)
        {
            //if (x > -1 && x < m_count_x && y > -1 && y < m_count_y && z > -1 && z < m_count_z)
            //{
            //    return m_arrChunks[x, y, z];
            //}
            //else return null;
            return null;
        }

        public Chunk GetChunk(Vector3Int Slot)
        {
            Chunk receiver;
            if (m_ChunksMap.TryGetValue(Slot, out receiver))
            {
                return receiver;
            }
            else return null;
        }
    }
}
