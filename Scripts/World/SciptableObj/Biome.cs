using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.World;
using MyNoise.Perlin;


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

    public int[,] GenerateHeightMap(int abs_x,int abs_z,int width,int depth,out int Rec_MaxHeight)
    {
        PerlinNoiseMaker noisemaker = new PerlinNoiseMaker();
        int[,] heightMap = new int[width, depth];
        int MaxHeight = 0;

        int i, j;
        for (i = 0; i < width; ++i)
        {
            for (j = 0; j < depth; ++j)
            {
                //heightMap[i, j] =(int)( Mathf.PerlinNoise((i + abs_x + Offset)*Frequency, (j + abs_z + Offset)*Frequency) * (Amplitude - GroundHeight));
                //heightMap[i, j] = (int)(noisemaker.GetNoise_2D_abs(new Vector2((i + abs_x + Offset) * Frequency, (j + abs_z + Offset) * Frequency)) * (Amplitude - GroundHeight));

                heightMap[i, j] = 
                    (int)noisemaker.GetNoise_2D_abs(new Vector2((i + abs_x + Offset), j + abs_z + Offset), Frequency, Amplitude-GroundHeight);
                heightMap[i, j] += (int)Amplitude - GroundHeight;

                MaxHeight = Math.Max(MaxHeight, heightMap[i, j]);
            }
        }
        Rec_MaxHeight = MaxHeight;
        return heightMap;
    }

    public LayerData getLayerData()
    {
        return m_LayerData;
    }
}
