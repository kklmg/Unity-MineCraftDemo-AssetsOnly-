using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Noise;
using Assets.Scripts.NPattern;

namespace Assets.Scripts.NWorld
{
    public class World : MonoBehaviour, IWorld
    {
        //Filed
        //--------------------------------------------------------------------
        //Section Size 
        [SerializeField]
        private ushort m_Section_Width = 16;
        [SerializeField]
        private ushort m_Section_Height = 16;
        [SerializeField]
        private ushort m_Section_Depth = 16;

        //chunk Height 
        [SerializeField]
        private ushort m_Chunk_Height = 16;

        [SerializeField]
        private uint m_Seed = 0xffffffff;

        private PerlinNoiseMaker m_NoiseMaker;

        //Biomes
        [SerializeField]
        private List<Biome> m_Bimoes;


        [SerializeField]
        private List<Block> m_BlockTypes;

        [SerializeField]
        private TextureSheet m_TextureSheet;

        private ChunkPool m_refPool;

        //Property
        //--------------------------------------------------------------------
        public ushort Section_Width { get { return m_Section_Width; } }
        public ushort Section_Height { get { return m_Section_Height; } }
        public ushort Section_Depth { get { return m_Section_Depth; } }
        public ushort Chunk_Height { get { return m_Chunk_Height; } }
        public uint Seed { get { return m_Seed; } }
        public ChunkPool Pool { get { return m_refPool; } }


        public PerlinNoiseMaker NoiseMaker { get { return m_NoiseMaker; } }
        public List<Block> BlockTypes { get { return m_BlockTypes; } }
        public TextureSheet TexSheet { get { return m_TextureSheet; } }
        public List<Biome> Biomes { get { return m_Bimoes; } }

        //unity function
        //--------------------------------------------------------------------
        private void Awake()
        {
            m_refPool = GetComponent<ChunkPool>();
            m_NoiseMaker = new PerlinNoiseMaker(m_Seed);
        }

        public Bounds GetBound(Vector3 Coord)
        {
            Bounds Temp = new Bounds();
            Vector3 vt = new Vector3(
                (Coord.x >= 0 ? (int)Coord.x : (int)Coord.x - 1),
                (Coord.y >= 0 ? (int)Coord.y : (int)Coord.y - 1),
                (Coord.z >= 0 ? (int)Coord.z : (int)Coord.z - 1));

            Temp.SetMinMax(vt, vt + Vector3.one);
            return Temp;
        }
    }
}