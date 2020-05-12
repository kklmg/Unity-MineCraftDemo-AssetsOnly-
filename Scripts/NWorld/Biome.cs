using System;
using System.Collections.Generic;

using UnityEngine;

using Assets.Scripts.NWorld;


[Serializable]
public struct BlockLayer
{
    [SerializeField]
    public int Top;
    public int Bottom;

    [SerializeField]
    public Block blockType;
}

[Serializable]
public class LayerData
{
    [SerializeField]
    private List<BlockLayer> m_Layers;

    private Dictionary<int,Block> m_LayerTable;

    [SerializeField]
    private Block DefaultBlock;
    [SerializeField]
    private Block baseBlock;

    [SerializeField]
    private byte baseBlockAltitude;

    public void Init()
    {
        m_LayerTable = new Dictionary<int, Block>();
        foreach (var layer in m_Layers)
        {
            for (int i = layer.Top; i < layer.Bottom; ++i)
            {
                m_LayerTable.Add(i,layer.blockType);
            }
        }
    }

    public Block GetBlock(int CurAltitude,int TopAltitude)
    {
        if (CurAltitude > TopAltitude) return null;
        if (CurAltitude < baseBlockAltitude) return baseBlock;

        if (m_LayerTable.ContainsKey(TopAltitude - CurAltitude))
        {
            return m_LayerTable[TopAltitude - CurAltitude];
        }
        else return DefaultBlock;
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

    public void Init()
    {
        m_LayerData.Init();
    }
}
