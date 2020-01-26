using UnityEngine;

namespace Assets.Scripts.NWorld
{
    [System.Serializable]
    public struct ChunkInWorld
    {
        //Constructor
        //----------------------------------------------------------------------------------
        public ChunkInWorld(Vector3 Coord, IWorld _world)
        {
            m_Value = new Vector2Int(
                Coord.x >= 0 ?
                    (int)(Coord.x / _world.Section_Width)
                    : (int)(Coord.x / _world.Section_Width) - 1,
                Coord.z >= 0 ?
                    (int)(Coord.z / _world.Section_Depth)
                    : (int)(Coord.z / _world.Section_Depth) - 1);
        }
        public ChunkInWorld(Vector2Int slot, IWorld _world)
        {
            m_Value = slot;
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
        [SerializeField]
        private Vector2Int m_Value;
    }

    [System.Serializable]
    public struct SectionInWorld
    {
        //Constructor
        //----------------------------------------------------------------------------------
        public SectionInWorld(Vector3 Coord, IWorld _world)
        {
            m_Value = new Vector3Int(
                Coord.x >= 0 ?
                    (int)(Coord.x / _world.Section_Width)
                    : (int)(Coord.x / _world.Section_Width) - 1,
                Coord.y >= 0 ?
                    (int)(Coord.y / _world.Section_Height)
                    : (int)(Coord.y / _world.Section_Height) - 1,
                Coord.z >= 0 ?
                    (int)(Coord.z / _world.Section_Depth)
                    : (int)(Coord.z / _world.Section_Depth) - 1);
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
            return new ChunkInWorld(new Vector2Int(m_Value.x,m_Value.z), _world);
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


        //Property
        //----------------------------------------------------------------------------------
        public Vector3Int Value { set { m_Value = value; } get { return m_Value; } }
        public int x { get { return m_Value.x; } }
        public int y { get { return m_Value.y; } }
        public int z { get { return m_Value.z; } }

        //Field
        //----------------------------------------------------------------------------------
        [SerializeField]
        public Vector3Int m_Value;
    }

    [System.Serializable]
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

        //Property
        //----------------------------------------------------------------------------------
        public int Value { set { m_Value = value; } get { return m_Value; } }

        //Field
        [SerializeField]
        private int m_Value;
    }

    [System.Serializable]
    public struct BlockInSection
    {
        //Constructor
        //----------------------------------------------------------------------------------
        public BlockInSection(Vector3 Coord, IWorld _world)
        {
            m_Value = new Vector3Int(
            Coord.x >= 0 ?
                (int)(Coord.x % _world.Section_Width)
                : (int)(Coord.x % _world.Section_Width) + _world.Section_Width - 1,
            Coord.y >= 0 ?
                (int)(Coord.y % _world.Section_Height)
                : (int)(Coord.y % _world.Section_Height) + _world.Section_Height - 1,
            Coord.z >= 0 ?
                (int)(Coord.z % _world.Section_Depth)
                : (int)(Coord.z % _world.Section_Depth) + _world.Section_Depth - 1
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

        public BlockInSection Offset(Vector3Int blkOffset, IWorld _world, out Vector3Int SecOffset)
        {
            Vector3Int dest = m_Value + blkOffset;

            _MapToValidRange(ref dest,_world,out SecOffset);


            return new BlockInSection(dest, _world);
        }

        public void Move(Vector3Int blkOffset, IWorld _world, out Vector3Int SecOffset)
        {
            m_Value += blkOffset;
            _MapToValidRange(ref m_Value, _world, out SecOffset);
        }

        //Private 
        private void _MapToValidRange(ref Vector3Int _value, IWorld _world, out Vector3Int SecOffset)
        {
            SecOffset = Vector3Int.zero;

            if (_value.x >= _world.Section_Width)
            {
                SecOffset.x = _value.x / _world.Section_Width;
                _value.x %= _world.Section_Width;
            }
            if (_value.y >= _world.Section_Height)
            {
                SecOffset.y = _value.y / _world.Section_Height;
                _value.y %= _world.Section_Height;
            }
            if (_value.z >= _world.Section_Depth)
            {
                SecOffset.z = _value.z / _world.Section_Depth;
                _value.z %= _world.Section_Depth;
            }

            if (_value.x < 0)
            {
                SecOffset.x = _value.x / _world.Section_Width - 1;
                _value.x = _value.x % _world.Section_Width + _world.Section_Width - 1;
            }
            if (_value.y < 0)
            {
                SecOffset.y = _value.y / _world.Section_Height - 1;
                _value.y = _value.y % _world.Section_Height + _world.Section_Height - 1;
            }
            if (_value.z < 0)
            {
                SecOffset.z = _value.z / _world.Section_Depth - 1;
                _value.z = _value.z % _world.Section_Depth + _world.Section_Depth - 1;
            }
        }

        //Property Value
        //----------------------------------------------------------------------------------
        public Vector3Int Value { set { m_Value = value; } get { return m_Value; } }
        public int x { get { return m_Value.x; } }
        public int y { get { return m_Value.y; } }
        public int z { get { return m_Value.z; } }
        //Field
        //----------------------------------------------------------------------------------
        [SerializeField]
        private Vector3Int m_Value;
    }

    [System.Serializable]
    public struct BlockLocation
    {
        [SerializeField]
        private ChunkInWorld m_ChunkInWorld; 
        [SerializeField]
        private SectionInWorld m_SecInWorld;
        [SerializeField]
        private BlockInSection m_BlkInSection;      

        [SerializeField]
        private Chunk m_Chunk;      //The Chunk Where block located in
        [SerializeField]
        private Section m_Section;  //The Section Where block located in
        [SerializeField]
        private Block m_Block;      //blockType
        [SerializeField]
        private Bounds m_Bound;     //Bound of Block used in handle collision

        //properties
        public ChunkInWorld ChunkInWorld { private set { m_ChunkInWorld = value;  } get { return m_ChunkInWorld; } }
        public SectionInWorld SecInWorld {private set { m_SecInWorld = value;  } get { return m_SecInWorld; } }
        public BlockInSection BlkInSec { private set { m_BlkInSection = value; } get { return m_BlkInSection; } }
        public Bounds Bound { private set { m_Bound = value; } get { return m_Bound; } }

        public void Update(IWorld _World)
        {
            //Get Chunk
            m_Chunk = _World.Pool.GetChunk(m_ChunkInWorld);
            if (m_Chunk == null) { m_Section = null; m_Block = null; }

            //Get Section 
            m_Section = m_Chunk.GetSection(m_SecInWorld.ToSectionInChunk());
            if (m_Section == null) { m_Block = null; }

            //Get Block
            m_Block = m_Section.GetBlock(m_BlkInSection);

        }

        public BlockLocation(Vector3 Coord, IWorld _World)
        {
            m_Bound = _World.GetBound(Coord);

            //Get Chunk Location
            m_ChunkInWorld = new ChunkInWorld(Coord, _World);
            //Get Section Location
            m_SecInWorld = new SectionInWorld(Coord, _World);
            //Get Block Location
            m_BlkInSection = new BlockInSection(Coord, _World);

            //Get Chunk
            m_Chunk = _World.Pool.GetChunk(m_ChunkInWorld);
            if (m_Chunk == null) { m_Section = null; m_Block = null; return; }

            //Get Section 
            m_Section = m_Chunk.GetSection(m_SecInWorld.ToSectionInChunk());
            if (m_Section == null) { m_Block = null; ; return; }

            //Get Block
            m_Block = m_Section.GetBlock(m_BlkInSection);
        }

        public void Set(Vector3 Coord, IWorld _World)
        {
            m_Bound = _World.GetBound(Coord);

            //Get Chunk Location
            m_ChunkInWorld = new ChunkInWorld(Coord, _World);
            //Get Section Location
            m_SecInWorld = new SectionInWorld(Coord, _World);
            //Get Block Location
            m_BlkInSection = new BlockInSection(Coord, _World);

            //Get Chunk
            m_Chunk = _World.Pool.GetChunk(m_ChunkInWorld);
            if (m_Chunk == null) { m_Section = null; m_Block = null; return; }

            //Get Section 
            m_Section = m_Chunk.GetSection(m_SecInWorld.ToSectionInChunk());
            if (m_Section == null) { m_Block = null; ; return; }

            //Get Block
            m_Block = m_Section.GetBlock(m_BlkInSection);
        }

        public BlockLocation Offset_Blk(Vector3Int offset,IWorld _world)
        {
            BlockLocation NewBlockLoc = default;
   
            NewBlockLoc.BlkInSec = m_BlkInSection.Offset(offset, _world, out Vector3Int SecOffset);
            NewBlockLoc.SecInWorld = m_SecInWorld.Offset(SecOffset);
            NewBlockLoc.Bound = new Bounds(m_Bound.center + offset,Vector3.one);
            NewBlockLoc.ChunkInWorld = NewBlockLoc.SecInWorld.ToChunkInWorld(_world);

            NewBlockLoc.Update(_world);

            return NewBlockLoc;
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
        public bool IsBlockExists()
        {
            return m_Block != null;
        }

    }
}
