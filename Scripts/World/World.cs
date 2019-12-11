using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.World
{
    class World : MonoBehaviour
    {
        //Filed-----------------------------------------------

        //Chunk Size 
        [SerializeField]
        private ushort chunk_width = 16;
        [SerializeField]
        private ushort chunk_height = 16;
        [SerializeField]
        private ushort chunk_depth = 16;

        //chunk count 
        [SerializeField]
        private ushort count_x = 2;
        [SerializeField]
        private ushort count_y = 1;
        [SerializeField]
        private ushort count_z = 1;




        [SerializeField]
        private List<Block> m_listBlocks;
        [SerializeField]
        private Chunk[,,] m_arrChunks;
        [SerializeField]
        private TextureSheet m_TextureSheet;

        //Property-------------------------------------------------------
        public ushort C_WIDTH { get { return chunk_width; } }
        public ushort C_HEIGHT { get { return chunk_height; } }
        public ushort C_DEPTH { get { return chunk_depth; } }

        public List<Block> BlockList { get { return m_listBlocks; } }
        public TextureSheet TexSheet { get { return m_TextureSheet; } }

        //unity function------------------------------------------------
        private void Start()
        {
            //Debug.Log("World_start");
            //Debug.Log("----------------------------------------------");
            //Debug.Log("block list size : "+ m_listBlocks.Count);
            //Debug.Log("----------------------------------------------");

            InitWorld();
        }


        //Function---------------------------------------------------
        void InitWorld()
        {
            m_arrChunks = new Chunk[count_x, count_y, count_z];

            GameObject go;
            int i, j, k;
            for (i = 0; i < count_x; ++i)
            {
                for (j = 0; j < count_y; ++j)
                {
                    for (k = 0; k < count_z; ++k)
                    {
                        //Instantiate<GameObject>()
                        go = new GameObject("Chunk" + '[' + i + ']' + '[' + j + ']' + '[' + k + ']');
                        go.AddComponent<Chunk>();

                        //set position
                        go.transform.position = new Vector3(i * chunk_width, j * chunk_height, k * chunk_depth);
                        go.transform.parent = this.transform;

                        //save chunk reference
                        Chunk refChunk = go.GetComponent<Chunk>();
                        m_arrChunks[i, j, k] = refChunk;

                        //set relative position
                        refChunk.WorldPos = new Vector3Int(i, j, k);


                    }
                }
            }
        }


       
        public Chunk GetChunk(int x,int y,int z)
        {
            if (x > -1 && x < count_x && y > -1 && y < count_y && z > -1 && z < count_z)
            {
                return m_arrChunks[x, y, z];
            }
            else return null;
        }

        public Chunk GetChunk(Vector3Int posWorld)
        {
            return GetChunk(posWorld.x, posWorld.y, posWorld.z);
        }
    }
}
