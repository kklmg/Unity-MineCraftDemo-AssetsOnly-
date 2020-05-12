using System.Collections.Generic;

using UnityEngine;

namespace Assets.Scripts.NWorld
{
    [CreateAssetMenu(menuName = "BlockPalette")]
    public class BlockPalette : ScriptableObject
    {
        public List<Block> m_BaseBlocks;    //Set at Unity Inspector

        private List<IBlock> m_Blocks;  //used in run time
        private Dictionary<IBlock, byte> m_Dic;

        public void Init()
        {
            m_Blocks = new List<IBlock>();

            foreach (var blk in m_BaseBlocks)
            {
                m_Blocks.Add(blk);
            }

            if (m_Blocks.Count > 256)
            {
                m_Blocks.Capacity = 256;
            }

            m_Dic = new Dictionary<IBlock, byte>();

            byte i = 0;
            foreach (var blk in m_Blocks)
            {
                if (blk == null)
                {
                    ++i;
                    continue;
                }
                else
                {
                    m_Dic.Add(blk, i++);
                }
            }
        }

        public IBlock this[byte ID]
        {
            get
            {
                if (ID == 0 || ID > m_Blocks.Count) return null;

                else return m_Blocks[ID];
            }
        }

        public byte GetBlockID(IBlock block)
        {
            if (block == null) return 0;

            if (m_Dic.TryGetValue(block, out byte res))
            {
                return res;
            }
            else if (m_Blocks.Count < 256)
            {
                m_Dic.Add(block, (byte)m_Blocks.Count);
                m_Blocks.Add(block);
                return (byte)(m_Blocks.Count - 1);
            }
            else return 0;
        }

        public int Count { get { return m_Blocks.Count; } }
    }
}
