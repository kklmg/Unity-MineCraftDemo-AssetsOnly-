using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Noise;
using MyNoise.Perlin;

namespace Assets.Scripts.World
{
    public struct ChunkColumn
    {
        //Field------------------------------------------------------------------------
        private World m_refWorld;
    
        [SerializeField]
        private int m_pos_x;
        private int m_pos_z;

        Biome m_refBiome;

        //[SerializeField]
        private Chunk[] m_arrChunks;  //Chunks

        //property----------------------------------------------------------------------
        //public bool isDirtry { get; set; }

        //public Vector2Int WorldPos { get { return m_PosWorld; } set { m_PosWorld = value; } }

        //Constructor------------------------------------------------------------------------------
        public ChunkColumn(int x, int z, World RefWorld,Transform parent)
        {
            m_pos_x = x;
            m_pos_z = z;
            //m_PosWorld = new Vector2Int(x, z);
            m_refWorld = RefWorld;
            m_arrChunks = new Chunk[RefWorld.C_HEIGHT];

            m_refBiome = null;

            CreateChunkSections(parent);
        }
        public void CreateChunkSections(Transform parent)
        {
            //Create Height Map 
            int[,] heightmap = m_refBiome.GenerateHeightMap(m_pos_x,m_pos_z,m_refWorld.C_WIDTH,m_refWorld.C_HEIGHT);

            var LayerIter = m_refBiome.GetLayerIter();
            
            GameObject NewGo;
            for (int i = 0; i < m_refWorld.C_HEIGHT; ++i)
            {
                NewGo = new GameObject("Chunk" + '[' + m_pos_x + ']' + '[' + m_pos_z + ']' + '[' + i + ']');
                NewGo.transform.parent = parent;

                NewGo.transform.position = new Vector3(m_pos_x, i * m_refWorld.C_HEIGHT, m_pos_z);
                m_arrChunks[i] = NewGo.AddComponent<Chunk>();



               // m_arrChunks[i].WorldPos

            }
        }
    }
}

