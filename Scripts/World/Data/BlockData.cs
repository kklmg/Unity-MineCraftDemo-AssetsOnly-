using Assets.Scripts.NWorld;
using Assets.Scripts.NWorld.Data;
using UnityEngine;

namespace Assets.Scripts.NWorld.Data
{
    public struct BlockPosition
    {
        readonly Section _Section;
        readonly Vector3Int _BlockSlot;

        public BlockPosition(Section sec, Vector3Int blkslot)
        {
            _Section = sec;
            _BlockSlot = blkslot;
        }

        public byte CurBlock
        {
            get
            {
                Debug.Assert(_Section != null);
                return _Section.GetBlockID(_BlockSlot);
            }
            set
            {
                Debug.Assert(_Section != null);
                _Section.SetBlock(_BlockSlot, value);
            }
        }
    }
}

