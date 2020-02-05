using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.NNoise;
using UnityEngine;

namespace Assets.Scripts.NWorld
{
    [Serializable]
    public class ChunkHeightMap
    {
        int[,] m_MapData;       //Height map data

        public int MinHeight { private set; get; }
        public int MaxHeight { private set; get; }

        //Indexer
        public int this[int i, int j]
        {
            get
            {
                return m_MapData[i, j];
            }
        }

        //Constructor
        public ChunkHeightMap(IWorld world)
        {
            m_MapData = new int[world.Section_Width, world.Section_Depth];
        }     

        public void Generate(Vector2Int coord,Biome biome,IWorld world)
        {
            MaxHeight = int.MinValue;
            MinHeight = int.MaxValue;

            INoiseMaker noiseMaker = world.NoiseMaker;

            int i, j;
            for (i = 0; i < world.Section_Width; ++i)
            {
                for (j = 0; j < world.Section_Depth; ++j)
                {
                    //Generate HeightMap use noise Maker
                    m_MapData[i, j] =
                        (int)noiseMaker.MakeOctave_2D
                        (new Vector2(i + coord.x + biome.Offset, j + coord.y + biome.Offset)   //coord
                        , biome.Frequency    //Frequecy
                        , biome.Amplitude - biome.GroundHeight) + biome.GroundHeight;    //Height


                    //Save Max and Min Height of This Chunk
                    MaxHeight = Math.Max(MaxHeight, m_MapData[i, j]);
                    MinHeight = Math.Min(MinHeight, m_MapData[i, j]);
                }
            }
        }
    }
}
