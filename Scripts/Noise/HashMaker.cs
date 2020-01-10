using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Noise
{
    public class HashMaker
    {
        readonly uint BIT_NOISE1;
        readonly uint BIT_NOISE2;
        readonly uint BIT_NOISE3;

        //cache
        private uint m_Cache_Uint;

        public HashMaker(uint noise)
        {
            BIT_NOISE1 = noise;
            BIT_NOISE2 = 0xcf87c3e6;
            BIT_NOISE3 = 0x7a39ccf3;
        }

        public int GetHash(int key)
        {
            m_Cache_Uint = (uint)key;
            m_Cache_Uint *= BIT_NOISE1;
            m_Cache_Uint ^= (m_Cache_Uint >> 8);
            m_Cache_Uint += BIT_NOISE2;
            m_Cache_Uint ^= (m_Cache_Uint << 8);
            m_Cache_Uint *= BIT_NOISE3;
            m_Cache_Uint ^= (m_Cache_Uint >> 8);

            return (int)m_Cache_Uint;
        }
    }

    //delegate of hash funcionn
    public delegate int del_HashFunction(int key);

}
