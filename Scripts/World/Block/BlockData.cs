using Assets.Scripts.NWorld;
using UnityEngine;

namespace Assets.Scripts.NWorld
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
        public byte CurBlockID
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

        public Block CurBlockRef
        {
            get
            {
                if (_Section == null) return null;
                else return _Section.GetBlock(_BlockSlot.x, _BlockSlot.y,_BlockSlot.z);
            }
        }
    }
}

