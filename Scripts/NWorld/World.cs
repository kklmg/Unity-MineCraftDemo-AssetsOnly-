using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.NNoise;
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

        //Biomes
        [SerializeField]
        private List<Biome> m_Bimoes;


        [SerializeField]
        private List<Block> m_BlockTypes;

        [SerializeField]
        private TextureSheet m_TextureSheet;

        //Property
        //--------------------------------------------------------------------
        public ushort Section_Width { get { return m_Section_Width; } }
        public ushort Section_Height { get { return m_Section_Height; } }
        public ushort Section_Depth { get { return m_Section_Depth; } }
        public ushort Chunk_Height { get { return m_Chunk_Height; } }
        public uint Seed { get { return m_Seed; } }
        public ChunkPool Pool { get; private set; }


        public INoiseMaker NoiseMaker { get; private set; }
        public IHashMaker HashMaker { get; private set; }

        public List<Block> BlockTypes { get { return m_BlockTypes; } }
        public TextureSheet TexSheet { get { return m_TextureSheet; } }
     

        //unity function
        //--------------------------------------------------------------------
        private void Awake()
        {
            Pool = GetComponent<ChunkPool>();

            //Init Hash Maker
            HashMaker = new HashMakerBase(m_Seed);
            //Init Noise Maker
            NoiseMaker = new PerlinNoiseMaker(HashMaker);
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

        public Biome GetBiome(ChunkInWorld chunkInWorld)
        {         
            return m_Bimoes[HashMaker.GetHash_2D(chunkInWorld.Value) % m_Bimoes.Count];
        }
    }
}