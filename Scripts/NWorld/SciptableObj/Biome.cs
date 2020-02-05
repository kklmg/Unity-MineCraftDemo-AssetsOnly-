using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.NWorld;
using Assets.Scripts.NNoise;


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
    
    public LayerData Layer { get { return m_LayerData; } }

    void Start()
    {
        m_LayerData.Init();
    }
}
