using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.World;
using MyNoise.Perlin;


[Serializable]
public struct BlockLayer :IComparable<BlockLayer>
{
    public float proportion;
    public int weight;
    public int blockID;
    private int TotalWeight;

    public int CompareTo(BlockLayer other)
    {
        return this.proportion < other.proportion ? 1 : -1;
    }
}

[CreateAssetMenu(menuName ="Biome")]
public class Biome : ScriptableObject
{
    public BlockLayer[] Layers;
    public int[] asdf;
    public float Offset;
    public float Frequency;
    public float Amplitude;

    // Start is called before the first frame update
    public IEnumerator GetLayerIter()
    {
        return Layers.GetEnumerator();
    }

    void Start()
    {
        Layers.GetLowerBound(1);
        
        //TotalWeight;



    }

    public int GetMappedBlockID(float target)
    {
        int low = 0;
        int high = Layers.Length;

        while (low < high)
        {
            int mid = low + (high - low) / 2;

            if (target > Layers[mid].proportion)
                low = mid + 1;
            else if (target < Layers[mid].proportion) 
                high = mid - 1;
            else
                return mid;
        }
        return Layers[low].blockID;  
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public int[,] GenerateHeightMap(int x,int z,int width,int depth)
    {
        PerlinNoiseMaker noisemaker = new PerlinNoiseMaker();
        int[,] heightMap = new int[width, depth];

        int i, j;
        int imax = width + x, jmax = depth + z;
        for (i = 0; i < imax; ++i)
        {
            for (j = 0; j < jmax; ++j)
            {
                heightMap[i, j] = 
                    (int)noisemaker.GetNoise_2D_abs(new Vector2((i + x + Offset), j + z + Offset), Frequency, Amplitude);
            }
        }
        return heightMap;
    }
}
