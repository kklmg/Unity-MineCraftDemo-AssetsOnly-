using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Runtime.InteropServices;

namespace Assets.Scripts.Pattern
{
    public abstract class ObjectPool<T> : MonoBehaviour where T : /*ILifeCycle,*/new()
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct PoolObj
        {
            [FieldOffset(0)]
            public T obj;
            [FieldOffset(0)]
            public int NextAvailable;
        }

        public PoolObj[] m_arrObj;

        private readonly int m_nPoolSize;
        private int m_nFirstAvailable;
        
        
        public ObjectPool(int size)
        {    
            m_nPoolSize = size;          
            m_arrObj = new PoolObj[m_nPoolSize];

            m_nFirstAvailable = 0;
            for (int i = 0; i < m_nPoolSize-1; ++i)
            {
                m_arrObj[i].NextAvailable = i + 1;
            }
            m_arrObj[m_nPoolSize - 1].NextAvailable = -1;
        }

        private bool PutObject()
        {
            if (m_nFirstAvailable == -1) return false;

            int NewID = m_nFirstAvailable;
            m_nFirstAvailable = m_arrObj[m_nFirstAvailable].NextAvailable;

            m_arrObj[NewID] = new PoolObj();

            return true;
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
