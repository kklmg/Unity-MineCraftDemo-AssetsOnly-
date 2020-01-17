using Assets.Scripts.NWorld;
using UnityEngine;

namespace Assets.Scripts.NWorld
{
    public struct BlockPosition
    {
        private readonly Section _Section;
        private readonly Vector3Int _BlockSlot;
        private readonly Bounds _Bound;

        public Bounds Bound { get { return _Bound; } }

        public BlockPosition(Section sec, Vector3Int blkslot,Bounds bound)
        {
            _Section = sec;
            _BlockSlot = blkslot;
            _Bound = bound;
        }
        public byte CurBlockID
        {
            get
            {
                if (_Section == null) return 0;
                return _Section.GetBlockID(_BlockSlot);
            }
            set
            {
                if (_Section != null)
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

