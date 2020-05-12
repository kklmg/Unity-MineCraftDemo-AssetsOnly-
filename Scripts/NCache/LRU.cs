using System.Collections.Generic;

namespace Assets.Scripts.NCache
{
    class LRUCache<TKEY,TVALUE>
    {
        //Field
        //-----------------------------------------------------------------
        private uint m_Capacity;    //Cache Capicity

        Dictionary<TKEY, TVALUE> m_Cache;   
        Dictionary<TKEY, LinkedListNode<TKEY>> m_LRU_POS;
        LinkedList<TKEY> m_LRU;

        //Constructor
        //-----------------------------------------------------------------
        public LRUCache(uint capacity)
        {
            m_Cache = new Dictionary<TKEY, TVALUE>();
            m_LRU_POS = new Dictionary<TKEY, LinkedListNode<TKEY>>();
            m_LRU = new LinkedList<TKEY>();

            m_Capacity = capacity;
        }


        //Puvblic Function
        //-----------------------------------------------------------------
        public bool TryGetValue(TKEY key,out TVALUE Value)
        {
            //int Value;
            if (m_Cache.TryGetValue(key, out Value))
            {
                _UpdateLRU(key);
                Value = m_Cache[key];

                return true;
            }
            return false;
        }


        public TVALUE Pop_LRU()
        {
            TVALUE Temp = m_Cache[m_LRU.Last.Value];
            _Evict();
            return Temp;
        }

        //Get least rencently used data
        public TVALUE GetLRUData()
        {
            TKEY TempKey = m_LRU.Last.Value;
            _UpdateLRU(m_LRU.Last.Value);
            return m_Cache[TempKey];
        }

        //try to put data to cache. if no enough space remove LRU(least rencently used) Data
        public void Put(TKEY key, TVALUE value)
        {
            if (m_Cache.Count == m_Capacity && m_Cache.TryGetValue(key, out TVALUE Value))
            {
                _Evict();
            }
            _UpdateLRU(key);
            m_Cache[key] = value;
        }

        public bool TryPut(TKEY key, TVALUE value)
        {
            if (m_Cache.Count == m_Capacity)
                return false;

            _UpdateLRU(key);
            m_Cache[key] = value;

            return true;
        }


        public bool IsFull()
        {
            return m_Cache.Count == m_Capacity;
        }

        //Private Function
        //-----------------------------------------------------------------

        private void _UpdateLRU(TKEY key)
        {
            if (m_Cache.TryGetValue(key, out TVALUE Value))
            {
                m_LRU.Remove(m_LRU_POS[key]);
            }
            //Move Current Value to first of lru list
            m_LRU.AddFirst(key);

            //Save key Position
            m_LRU_POS[key] = m_LRU.First;
        }

        private void _Evict()
        {
            m_LRU_POS.Remove(m_LRU.Last.Value);
            m_Cache.Remove(m_LRU.Last.Value);
            m_LRU.RemoveLast();
        }

    };
}
