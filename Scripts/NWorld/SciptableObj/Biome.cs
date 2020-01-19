using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.NWorld;
using Assets.Scripts.Noise;


[Serializable]
public struct BlockLayer :IComparable<BlockLayer>
{
    [SerializeField]
    public int offset;

    [SerializeField]
    public byte blockID;

    public int CompareTo(BlockLayer other)
    {
        return this.offset < other.offset ? 1 : -1;
    }
}

[Serializable]
public class LayerData
{
    [SerializeField]
    private List<BlockLayer> m_Layers;

    [SerializeField]
    private byte DefaultBlockID;
    [SerializeField]
    private byte baseBlockID;
    [SerializeField]
    private byte baseBlockHeight;

    public void Init()
    {
        m_Layers.Sort();
    }

    public byte GetBlockID(int CurHeight,int MaxHeight)
    {
        if (CurHeight > MaxHeight) return 0;
        if (CurHeight < baseBlockHeight) return baseBlockID;

        foreach (var layer in m_Layers)
        {
            if (CurHeight > MaxHeight - layer.offset)
                return layer.blockID;
        }
        return DefaultBlockID;
    }
}




[CreateAssetMenu(menuName ="Biome")]
public class Biome : ScriptableObject
{
    [SerializeField]
    public LayerData m_LayerData;

    [SerializeField]
    public int GroundHeight;

    [SerializeField]
    public float Offset;

    [SerializeField]
    public float Frequency;

    [SerializeField]
    public float Amplitude;


    void Start()
    {
        m_LayerData.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int[,] GenerateHeightMap(Vector2Int coord,int width,int depth,PerlinNoiseMaker NoiseMaker,
        out int Out_MaxHeight,out int Out_MinHeight)
    {
       
        int[,] heightMap = new int[width, depth];
        int MaxHeight = 0;
        int MinHeight = (int)Amplitude;

        int i, j;
        for (i = 0; i < width; ++i)
        {
            for (j = 0; j < depth; ++j)
            {
                //Generate HeightMap use noise Maker
                heightMap[i, j] = 
                    (int)NoiseMaker.GetOctaveNoise_2D(new Vector2((i + coord.x + Offset), j + coord.y + Offset), Frequency, Amplitude-GroundHeight);

                heightMap[i, j] += GroundHeight;

                //Save Max and Min Height of This Chunk
                MaxHeight = Math.Max(MaxHeight, heightMap[i, j]);
                MinHeight = Math.Min(MinHeight, heightMap[i, j]);
            }
        }
        Out_MaxHeight = MaxHeight;
        Out_MinHeight = MinHeight;
        return heightMap;
    }

    public LayerData getLayerData()
    {
        return m_LayerData;
    }
}
