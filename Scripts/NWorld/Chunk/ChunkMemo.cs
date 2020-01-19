using System;
using System.Collections.Generic;
using Assets.Scripts.NCommand.Impl;

namespace Assets.Scripts.NWorld
{
    [Serializable]
    public struct ChunkMemo
    {
        List<Com_ChangeBlock> m_Changes;

        void add(Com_ChangeBlock _command)
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
