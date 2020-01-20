using UnityEngine;

namespace Assets.Scripts.NWorld
{
    public struct ChunkInWorld
    {
        //Constructor
        //----------------------------------------------------------------------------------
        public ChunkInWorld(Vector3 Coord, IWorld _world)
        {
            m_Value = new Vector2Int(
                Coord.x >= 0 ?
                    (int)Coord.x / _world.Section_Width
                    : (int)(Coord.x) / _world.Section_Width - 1,
                Coord.y >= 0 ?
                    (int)Coord.y / _world.Section_Depth
                    : (int)(Coord.y) / _world.Section_Depth - 1);
        }
        public ChunkInWorld(int x, int y, IWorld _world)
        {
            m_Value = new Vector2Int(
                x >= 0 ?
                    x / _world.Section_Width
                    : x / _world.Section_Width - 1,
                y >= 0 ?
                    y / _world.Section_Depth
                    : y / _world.Section_Depth - 1);
        }


        //Convert functions
        //-------------------------------------------------------------------------
        public Vector2 ToCoord2D(IWorld _world)
        {
            return new Vector2(
             m_Value.x * _world.Section_Width,
             m_Value.y * _world.Section_Depth);
        }
        public Vector3 ToCoord3D(IWorld _world)
        {
            return new Vector3(
                m_Value.x * _world.Section_Width,
                0,
                m_Value.y * _world.Section_Depth);
        }
        public Vector2Int ToCoord2DInt(IWorld _world)
        {
            return new Vector2Int(
             m_Value.x * _world.Section_Width,
             m_Value.y * _world.Section_Depth);
        }
        public Vector3Int ToCoord3DInt(IWorld _world)
        {
            return new Vector3Int(
                m_Value.x * _world.Section_Width,
                0,
                m_Value.y * _world.Section_Depth);
        }


        //Property 
        //----------------------------------------------------------------------------------
        public Vector2Int Value { set { m_Value = value; } get { return m_Value; } }
        public int x { get { return m_Value.x; } }
        public int y { get { return m_Value.y; } }
        //Field
        //----------------------------------------------------------------------------------
        private Vector2Int m_Value;
    }
    public struct SectionInWorld
    {
        //Constructor
        //----------------------------------------------------------------------------------
        public SectionInWorld(Vector3 Coord, IWorld _world)
        {
            m_Value = new Vector3Int(
                Coord.x >= 0 ?
                    (int)Coord.x / _world.Section_Width
                    : (int)(Coord.x) / _world.Section_Width - 1,
                Coord.y >= 0 ?
                    (int)Coord.y / _world.Section_Height
                    : (int)(Coord.y) / _world.Section_Height - 1,
                Coord.z >= 0 ?
                    (int)Coord.z / _world.Section_Depth
                    : (int)(Coord.z) / _world.Section_Depth - 1);
        }
        public SectionInWorld(int x, int y, int z)
        {
            m_Value = new Vector3Int(x, y, z);
        }
        public SectionInWorld(Vector3Int SecSlot)
        {
            m_Value = SecSlot;
        }

        //Convert functions
        //-------------------------------------------------------------------------
        public Vector3 ToCoord(IWorld _world)
        {
            return new Vector3(
               (float)m_Value.x * _world.Section_Width,
               (float)m_Value.y * _world.Section_Height,
               (float)m_Value.z * _world.Section_Depth);

        }
        public ChunkInWorld ToChunkInWorld(IWorld _world)
        {
            return new ChunkInWorld(m_Value.x, m_Value.z, _world);
        }
        public SectionInChunk ToSectionInChunk()
        {
            return new SectionInChunk(m_Value.y);
        }

        //Translate
        public Vector3Int Move(Vector3Int offset)
        {
            m_Value += offset;
            return m_Value;
        }
        public SectionInWorld Offset(Vector3Int offset)
        {
            return new SectionInWorld(m_Value + offset);
        }


        //Property Value
        //----------------------------------------------------------------------------------
        public Vector3Int Value { set { m_Value = value; } get { return m_Value; } }
        public int x { get { return m_Value.x; } }
        public int y { get { return m_Value.y; } }
        public int z { get { return m_Value.z; } }
        //Field
        //----------------------------------------------------------------------------------
        public Vector3Int m_Value;
    }
    public struct SectionInChunk
    {
        //Constructor
        //----------------------------------------------------------------------------------
        public SectionInChunk(Vector3 Coord, IWorld _world)
        {
            m_Value = (int)(Coord.y / _world.Section_Height);
        }
        public SectionInChunk(int Slot)
        {
            m_Value = Slot;
        }

        //Property Value
        //----------------------------------------------------------------------------------
        public int Value { set { m_Value = value; } get { return m_Value; } }

        private int m_Value;
    }
    public struct BlockInSection
    {
        //Constructor
        //----------------------------------------------------------------------------------
        public BlockInSection(Vector3 Coord, IWorld _world)
        {
            m_Value = new Vector3Int(
            (Coord.x >= 0 ?
                (int)Coord.x % _world.Section_Width
                : (int)Coord.x % _world.Section_Width + _world.Section_Width - 1),
            (Coord.y >= 0 ?
                (int)Coord.y % _world.Section_Height
                : (int)Coord.y % _world.Section_Height + _world.Section_Height - 1),
            (Coord.z >= 0 ?
                (int)Coord.z % _world.Section_Depth
                : (int)Coord.z % _world.Section_Depth + _world.Section_Depth - 1)
            );
        }
        public BlockInSection(Vector3Int slot, IWorld _world)
        {
            m_Value = new Vector3Int(
            (slot.x >= 0 ?
                slot.x % _world.Section_Width
                : slot.x % _world.Section_Width + _world.Section_Width - 1),
            (slot.y >= 0 ?
                slot.y % _world.Section_Height
                : slot.y % _world.Section_Height + _world.Section_Height - 1),
            (slot.z >= 0 ?
                slot.z % _world.Section_Depth
                : slot.z % _world.Section_Depth + _world.Section_Depth - 1)
            );
        }
        public BlockInSection(int x, int y, int z, IWorld _world)
        {
            m_Value = new Vector3Int(
            (x >= 0 ?
                x % _world.Section_Width
                : x % _world.Section_Width + _world.Section_Width - 1),
            (y >= 0 ?
                y % _world.Section_Height
                : y % _world.Section_Height + _world.Section_Height - 1),
            (z >= 0 ?
                z % _world.Section_Depth
                : z % _world.Section_Depth + _world.Section_Depth - 1)
            );
        }

        public bool IsInSectionBorder(IWorld _world)
        {
            if (Value.x == _world.Section_Width - 1 || Value.x == 0) return true;
            if (Value.y == _world.Section_Width - 1 || Value.y == 0) return true;
            if (Value.z == _world.Section_Width - 1 || Value.z == 0) return true;
            return false;
        }

        public Vector3Int Offset(Vector3Int blkOffset, IWorld _world, out Vector3Int SecOffset)
        {
            SecOffset = Vector3Int.zero;

            Vector3Int dest = m_Value + blkOffset;
            //Value += offset;

            if (dest.x > _world.Section_Width)
            {
                SecOffset.x /= _world.Section_Width;
                dest.x %= _world.Section_Width;
            }
            if (dest.y > _world.Section_Height)
            {
                SecOffset.y /= _world.Section_Height;
                dest.y %= _world.Section_Height;
            }
            if (dest.z > _world.Section_Depth)
            {
                SecOffset.z /= _world.Section_Depth;
                dest.z %= _world.Section_Depth;
            }

            if (dest.x < 0)
            {
                SecOffset.x = SecOffset.x / _world.Section_Width - 1;
                dest.x = dest.x % _world.Section_Width + _world.Section_Width - 1;
            }
            if (dest.y < 0)
            {
                SecOffset.y = SecOffset.y / _world.Section_Height - 1;
                dest.y = dest.y % _world.Section_Height + _world.Section_Height - 1;
            }
            if (dest.z < 0)
            {
                SecOffset.z = SecOffset.z / _world.Section_Depth - 1;
                dest.z = dest.z % _world.Section_Depth + _world.Section_Depth - 1;
            }

            return Value;
        }
        public void Move(Vector3Int blkOffset, IWorld _world, out Vector3Int SecOffset)
        {
            m_Value = Offset(blkOffset, _world, out SecOffset);
        }

        //Property Value
        //----------------------------------------------------------------------------------
        public Vector3Int Value { set { m_Value = value; } get { return m_Value; } }
        public int x { get { return m_Value.x; } }
        public int y { get { return m_Value.y; } }
        public int z { get { return m_Value.z; } }
        //Field
        //----------------------------------------------------------------------------------
        private Vector3Int m_Value;
    }


    public struct BlockLocation
    {
        private ChunkInWorld m_ChunkInWorld;
        private SectionInWorld m_SecInWorld;
        private BlockInSection m_BlkInSection;

        private Chunk m_Chunk;
        private Section m_Section;
        private Block m_Block;

        private Bounds m_Bound;

        public Bounds Bound { get { return m_Bound; } }

        public void SetLocation(Vector3 Coord, IWorld _World)
        {
            //Get Chunk Location
            m_ChunkInWorld = new ChunkInWorld(Coord, _World);
            m_Chunk = _World.Pool.GetChunk(new ChunkInWorld(Coord, _World));
            if (m_Chunk == null) return;

            //Get Section Location
            m_SecInWorld = new SectionInWorld(Coord, _World);
            m_Section = m_Chunk.GetSection(m_SecInWorld.ToSectionInChunk());
            if (m_Section == null) return;

            //Get Block Location
            m_BlkInSection = new BlockInSection(Coord, _World);
            m_Block = m_Section.GetBlock(m_BlkInSection);
            if (m_Block == null) return;

            m_Bound = _World.GetBound(Coord);
        }

        public void Reset()
        {
            m_Chunk = null;
            m_Section = null;
            m_Block = null;
        }

        public byte CurBlockID
        {
            get
            {
                if (m_Section == null) return 0;
                return m_Section.GetBlockID(m_BlkInSection);
            }
            set
            {
                if (m_Section != null)
                    m_Section.SetBlock(m_BlkInSection, value);
            }
        }
        public Block CurBlockRef
        {
            get
            {
                if (m_Section == null) return null;
                else return m_Section.GetBlock(m_BlkInSection);
            }
        }

        public bool IsValid()
        {
            if (m_Chunk == null || m_Section == null || m_Block == null) return false;
            else return true;
        }
    }
}
