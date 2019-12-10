using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.World
{
    class World : MonoBehaviour
    {





        [SerializeField]
        private List<Block> m_listBlocks;
        public List<Block> BlockList { get { return m_listBlocks; } }

        private Chunk[] m_arrChunks;

        public TextureSheet m_TextureSheet;

        void CreateWorld()
        {
            //m_arrChunks = new Chunk[5];


        }
        private void Start()
        {
            m_arrChunks = new Chunk[10];

            GameObject go;
            for (int i = 0; i < 10; ++i)
            {
                //Instantiate<GameObject>()
                go = new GameObject("Chunk ["+i+']');
                go.transform.position = new Vector3(i*15,0,0);
                go.transform.parent = this.transform;
                go.AddComponent<Chunk>();
            }
        }


    }
}
