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
        private int m_WorldSlot_x;
        [SerializeField]
        private int m_WorldSlot_z;
        [SerializeField]
        private int m_Coord_abs_x;
        [SerializeField]
        private int m_Coord_abs_z;

        Biome m_refBiome;
        Transform m_Parent;

        [SerializeField]
        int[,] m_arrHeightMap;

        //[SerializeField]
        private Chunk[] m_arrChunks;  //Chunks

        //property----------------------------------------------------------------------
        //public bool isDirtry { get; set; }

        //public Vector2Int WorldPos { get { return m_PosWorld; } set { m_PosWorld = value; } }

        //Constructor------------------------------------------------------------------------------
        public ChunkColumn(int slot_x, int slot_z, World RefWorld, Transform parent, Biome RefBiome)
        {
            m_WorldSlot_x = slot_x;
            m_WorldSlot_z = slot_z;

            m_Coord_abs_x = slot_x * RefWorld.C_WIDTH;
            m_Coord_abs_z = slot_z * RefWorld.C_DEPTH;

            //m_PosWorld = new Vector2Int(x, z);
            m_refWorld = RefWorld;
            m_arrChunks = new Chunk[RefWorld.C_HEIGHT];

            m_Parent = parent;
            m_refBiome = RefBiome;

            m_arrHeightMap = m_refBiome.GenerateHeightMap(m_Coord_abs_x, m_Coord_abs_z, m_refWorld.C_WIDTH, m_refWorld.C_HEIGHT);

            CreateAllChunkSections();
        }

        //create instance of chunk
        public void CreateChunkSection(int slot_y)
        {
            if (slot_y * m_refWorld.C_HEIGHT > m_refBiome.Amplitude) return;

            GameObject NewGo = 
                new GameObject("Chunk" + '[' + m_Coord_abs_x + ']' + '[' + m_Coord_abs_z + ']' + '[' + slot_y + ']');
            NewGo.transform.parent = m_Parent;

            NewGo.transform.position = new Vector3(m_Coord_abs_x, slot_y * m_refWorld.C_HEIGHT, m_Coord_abs_z);

            m_arrChunks[slot_y] = NewGo.AddComponent<Chunk>();
            m_arrChunks[slot_y].WorldSlot = new Vector3Int(m_WorldSlot_x, slot_y,m_WorldSlot_z);
            m_arrChunks[slot_y].GenerateChunk(m_refBiome.getLayerData(), m_arrHeightMap, slot_y * m_refWorld.C_HEIGHT);
        }
        

        public void CreateAllChunkSections()
        {
            for (int i = 0; i < m_refWorld.C_HEIGHT; ++i)
            {
                CreateChunkSection(i);
            }
        }
    }
}

