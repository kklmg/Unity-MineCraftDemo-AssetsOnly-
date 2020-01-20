using System.Collections.Generic;
using Assets.Scripts.Noise;
using UnityEngine;

namespace Assets.Scripts.NWorld
{
    public interface IWorld
    {
        ushort Section_Width { get; }
        ushort Section_Height { get; }
        ushort Section_Depth { get; }
        ushort Chunk_Height { get; }
        uint Seed { get; }
        ChunkPool Pool { get; }
        Bounds GetBound(Vector3 Coord);

        PerlinNoiseMaker NoiseMaker { get; }
        List<Block> BlockTypes { get; }
        TextureSheet TexSheet { get; }
        List<Biome> Biomes { get; }
    }
}
