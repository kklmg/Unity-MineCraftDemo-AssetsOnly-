using Assets.Scripts.NWorld;
using Assets.Scripts.Pattern;
using UnityEngine;

namespace Assets.Scripts.NWorld
{
    public struct BlockPosition
    {
        private Chunk _Chunk;
        private Section _Section;
        private Vector3Int _SectionSlot;
        private Vector3Int _BlockSlot;
        private Bounds _Bound;

        public Bounds Bound { get { return _Bound; } }

        public BlockPosition(Vector3 coord)
        {
            World world = Locator<World>.GetService();

            _SectionSlot = world.CoordToSlot(coord);
            _Chunk = world.GetChunk(_SectionSlot);
            _Section = _Chunk.GetSection(_SectionSlot.y,true);
            _BlockSlot = world.CoordToBlockSlot(coord);
            _Bound = world.GetBound(coord);
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

        public void Move(Vector3Int offset)
        {
            World world = Locator<World>.GetService();

            _SectionSlot = _SectionSlot + offset;
            _Chunk = world.GetChunk(_SectionSlot);
            _Section = _Chunk.GetSection(_SectionSlot.y, true);
            _BlockSlot = world.CoordToBlockSlot(_SectionSlot);
            _Bound = world.GetBound(_SectionSlot);
        }



    }
}

