using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Runtime.InteropServices;

namespace Assets.Scripts.Pattern
{

    [StructLayout(LayoutKind.Explicit)]
    public struct PoolObj<T> where T: new()
    {
        [FieldOffset(0)]
        public T obj;
        [FieldOffset(0)]
        public int NextAvailable;
    }

    public abstract class ObjectPool<T> where T : /*ILifeCycle,*/new()
    {
       
        [SerializeField]
        protected PoolObj<T>[] m_arrObj;

        protected readonly int m_nPoolSize;
        protected int m_nFirstAvailable;


        protected ObjectPool(int size)
        {    
            m_nPoolSize = size;          
            m_arrObj = new PoolObj<T>[m_nPoolSize];

            m_nFirstAvailable = 0;
            for (int i = 0; i < m_nPoolSize-1; ++i)
            {
                m_arrObj[i].NextAvailable = i + 1;
            }
            m_arrObj[m_nPoolSize - 1].NextAvailable = -1;
        }

        private bool CreateObject()
        {
            if (m_nFirstAvailable == -1) return false;

            int NewID = m_nFirstAvailable;
            m_nFirstAvailable = m_arrObj[m_nFirstAvailable].NextAvailable;

            m_arrObj[NewID] = new PoolObj<T>();

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
