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
    public class Chunk : MonoBehaviour
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

        int m_MaxHeight;
        Biome m_refBiome;
        Transform m_Parent;

        [SerializeField]
        int[,] m_arrHeightMap;

        //[SerializeField]
        private Section[] m_arrSections;  //Sections

        //property----------------------------------------------------------------------
        //public bool isDirtry { get; set; }

        //public Vector2Int WorldPos { get { return m_PosWorld; } set { m_PosWorld = value; } }
        public World WorldReference { get { return m_refWorld; } }


        //Unity Funciton
        //------------------------------------------------------------------------------
        private void Awake()
        {
            m_refWorld = transform.parent.GetComponent<World>();
        }

        public void Init(int slot_x, int slot_z, World RefWorld, Transform parent, Biome RefBiome)
        {
            m_WorldSlot_x = slot_x;
            m_WorldSlot_z = slot_z;

            m_Coord_abs_x = slot_x * RefWorld.C_WIDTH;
            m_Coord_abs_z = slot_z * RefWorld.C_DEPTH;

            //m_PosWorld = new Vector2Int(x, z);
           

            if(m_arrSections == null)
            m_arrSections = new Section[RefWorld.C_HEIGHT];

            m_Parent = parent;
            m_refBiome = RefBiome;

            m_arrHeightMap = m_refBiome.GenerateHeightMap
                (m_Coord_abs_x, m_Coord_abs_z, m_refWorld.C_WIDTH, m_refWorld.C_HEIGHT, out m_MaxHeight);

            Debug.Log(m_MaxHeight);
            CreateAllSections();
        }

        //create instance of Section
        public void CreateSection(int slot_y)
        {          
            if (slot_y * m_refWorld.C_HEIGHT > m_MaxHeight) return;

            GameObject NewGo = new GameObject("Section" + '[' + slot_y + ']');
            NewGo.transform.parent = this.transform;

            NewGo.transform.position = new Vector3(m_Coord_abs_x, slot_y * m_refWorld.C_HEIGHT, m_Coord_abs_z);

            m_arrSections[slot_y] = NewGo.AddComponent<Section>();
            m_arrSections[slot_y].WorldSlot = new Vector3Int(m_WorldSlot_x, slot_y,m_WorldSlot_z);
            m_arrSections[slot_y].GenerateSection(m_refBiome.getLayerData(), m_arrHeightMap, slot_y * m_refWorld.C_HEIGHT);
        }
        

        public void CreateAllSections()
        {
            for (int i = 0; i < m_refWorld.C_HEIGHT; ++i)
            {
                CreateSection(i);
            }
            Debug.Log("section created");
        }
    }
}

