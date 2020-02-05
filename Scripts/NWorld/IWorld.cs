using System.Collections.Generic;
using Assets.Scripts.NNoise;
using UnityEngine;

namespace Assets.Scripts.NWorld
{
    public interface IWorld
    {
        //Section Size
        ushort Section_Width { get; }
        ushort Section_Height { get; }
        ushort Section_Depth { get; }

        //Chunk's Max Height
        ushort Chunk_Height { get; }

        uint Seed { get; }

        //Chunk Creator
        ChunkPool Pool { get; }

        INoiseMaker NoiseMaker { get; }
        IHashMaker HashMaker { get; }

        //block type array
        List<Block> BlockTypes { get; }

        //block textures
        TextureSheet TexSheet { get; }

        //Chunk Biome
        Biome GetBiome(ChunkInWorld chunkInWorld);

        Bounds GetBound(Vector3 Coord);
    }
}
