using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using UnityEngine;
using Assets.Scripts.Pattern;

namespace Assets.Scripts.World
{
    public class ChunkPool
    {
        //Field
        //---------------------------------------------------------------------------
        [SerializeField]
        //private Chunk[] m_arrChunks;
        //private Queue<int> m_FreeQue;
        private Dictionary<Vector2Int, Chunk> m_DicChunks;
        int m_MaxCount;
        //Constructor
        //---------------------------------------------------------------------------
        public ChunkPool(int size)
        {
            m_MaxCount = size;
            m_DicChunks = new Dictionary<Vector2Int, Chunk>();
        }

        //Public Function
        //---------------------------------------------------------------------------
        public void Spawn(int slot_x, int slot_z, World RefWorld, Transform parent, Biome RefBiome)
        {
            Chunk _Chunk;
            if (m_DicChunks.TryGetValue(new Vector2Int(slot_x, slot_z), out _Chunk)) return;
            else if (m_DicChunks.Count < m_MaxCount)
            {
                GameObject Go = new GameObject("Chunk" + "[" + slot_x + "]" + "[" + slot_z + "]");
                Go.transform.transform.position = new Vector3(RefWorld.C_WIDTH*slot_x, 0, RefWorld.C_DEPTH*slot_z);
                Go.transform.SetParent(parent);

                _Chunk = Go.AddComponent<Chunk>();
                _Chunk.Init(slot_x, slot_z, RefWorld, parent, RefBiome);

                m_DicChunks.Add(new Vector2Int(slot_x, slot_z), _Chunk);
            }
            else return;
        }



        private void Update()
        {

        }

    }


}
