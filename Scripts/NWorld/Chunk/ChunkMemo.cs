using System;
using System.Collections.Generic;

using Assets.Scripts.NCommand;

namespace Assets.Scripts.NWorld
{
    [Serializable]
    public struct ChunkMemo
    {
        List<Com_ModifyBlock> m_Changes;

        void add(Com_ModifyBlock _command)
        {
            m_Changes.Add(_command);
        }

        void pop_back()
        {
            if (m_Changes.Count == 0) return;
            m_Changes.RemoveAt(m_Changes.Count - 1);
        }
        void clear()
        {
            m_Changes.Clear();
        }
    }
}
