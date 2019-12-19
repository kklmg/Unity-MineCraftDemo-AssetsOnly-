using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Assets.Scripts.World
{
    [StructLayout(LayoutKind.Explicit)]
    public struct PoolObj_ck
    {
        [FieldOffset(0)]
        public ChunkColumn obj;
        [FieldOffset(0)]
        public int NextAvailable;
    }

    public class ChunkColPool
    {
        //Field
        //---------------------------------------------------------------------------
        [SerializeField]
        private PoolObj_ck[] m_arrObj;

        private readonly int m_nPoolSize;
        private int m_nFirstAvailable;

        //Constructor
        //---------------------------------------------------------------------------
        public ChunkColPool(int size)
        {
            m_nPoolSize = size;
            m_arrObj = new PoolObj_ck[m_nPoolSize];

            m_nFirstAvailable = 0;
            for (int i = 0; i < m_nPoolSize - 1; ++i)
            {
                m_arrObj[i].NextAvailable = i + 1;
            }
            m_arrObj[m_nPoolSize - 1].NextAvailable = -1;
        }

        //Public Function
        //---------------------------------------------------------------------------
        public void CreateChunkCol(int abs_x, int abs_z, World RefWorld, Transform parent, Biome RefBiome)
        {
            if (m_nFirstAvailable == -1) return;

            int NewID = m_nFirstAvailable;
            m_nFirstAvailable = m_arrObj[m_nFirstAvailable].NextAvailable;

            m_arrObj[NewID].obj = new ChunkColumn(abs_x, abs_z, RefWorld, parent, RefBiome);
        }

        private void Update()
        {
            //int length = m_arrObj.Length;
            //for (int i = 0; i < length; ++i)
            //{
            //    if (m_arrObj[i].obj.Tick() == false)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}


        }

    }


}
