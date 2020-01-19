using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Noise;
using Assets.Scripts.Pattern;

namespace Assets.Scripts.NWorld
{
    public interface IWorld
    {
        ushort Section_Width { get; }
        ushort Section_Height { get; }
        ushort Section_Depth { get; }
        ushort Chunk_Height { get; }
        ChunkPool Pool { get; }
    }

    public class World : MonoBehaviour, IWorld
    {
        //Filed
        //--------------------------------------------------------------------
        //Section Size 
        [SerializeField]
        private ushort m_Section_Width = 16;
        [SerializeField]
        private ushort m_Section_Height = 16;
        [SerializeField]
        private ushort m_Section_Depth = 16;

        //chunk Height 
        [SerializeField]
        private ushort m_Chunk_Height = 16;

        [SerializeField]
        private uint m_Seed = 0xffffffff;

        private PerlinNoiseMaker m_NoiseMaker;

        //Biomes
        [SerializeField]
        private List<Biome> m_Bimoes;


        [SerializeField]
        private List<Block> m_listBlocks;

        [SerializeField]
        private TextureSheet m_TextureSheet;

        private ChunkPool m_refPool;

        //Property
        //--------------------------------------------------------------------
        public ushort Section_Width { get { return m_Section_Width; } }
        public ushort Section_Height { get { return m_Section_Height; } }
        public ushort Section_Depth { get { return m_Section_Depth; } }
        public ushort Chunk_Height { get { return m_Chunk_Height; } }
        public uint Seed { get { return m_Seed; } }
        public ChunkPool Pool { get { return m_refPool; } }


        public PerlinNoiseMaker NoiseMaker { get { return m_NoiseMaker; } }
        public List<Block> BlockList { get { return m_listBlocks; } }
        public TextureSheet TexSheet { get { return m_TextureSheet; } }
        public List<Biome> Biomes { get { return m_Bimoes; } }

        //unity function
        //--------------------------------------------------------------------
        private void Awake()
        {
            m_refPool = GetComponent<ChunkPool>();
            m_NoiseMaker = new PerlinNoiseMaker(m_Seed);
        }

        public Bounds GetBound(Vector3 Coord)
        {
            Bounds Temp = new Bounds();
            Vector3 vt = new Vector3(
                (Coord.x >= 0 ? (int)Coord.x : (int)Coord.x - 1),
                (Coord.y >= 0 ? (int)Coord.y : (int)Coord.y - 1),
                (Coord.z >= 0 ? (int)Coord.z : (int)Coord.z - 1));

            Temp.SetMinMax(vt, vt + Vector3.one);
            return Temp;
        }
        public Chunk GetChunk(Vector3 Coord)
        {
            Vector3Int Slot = CoordToSlot(Coord);
            return m_refPool.GetChunk(new Vector2Int(Slot.x, Slot.z));
        }
        public Chunk GetChunk(Vector3Int Slot)
        {
            return m_refPool.GetChunk(new Vector2Int(Slot.x, Slot.z));
        }
        public Section GetSection(Vector3 Coord)
        {
            Vector3Int Slot = CoordToSlot(Coord);
            return GetSection(Slot);
        }
        public Section GetSection(Vector3Int Slot)
        {
            //Try get chunk
            Chunk _chunk = m_refPool.GetChunk(new Vector2Int(Slot.x, Slot.z));
            if (_chunk == null) return null;

            //Try get Section
            Section _section = _chunk.GetSection(s.y);
            if (_section == null) return null;

            return _section;
        }

        public Block GetBlock(Vector3 Coord)
        {
            Vector3Int SectionSlot = CoordToSlot(Coord);

            //Try get section
            Section section = GetSection(SectionSlot);
            if (section == null) return null;

            Vector3Int BlockSlot = CoordToBlockSlot(Coord);
            return section.GetBlock(BlockSlot.x, BlockSlot.y, BlockSlot.z);
        }

        public BlockPosition GetBlockPos(Vector3 Coord)
        {
            return new BlockPosition(Coord);
        }

        public bool TryGetBlockPos(Vector3 Coord, out BlockPosition blkpos)
        {
            blkpos = default(BlockPosition);

            Vector3Int SectionSlot = CoordToSlot(Coord);

            //Try get section
            Section section = GetSection(SectionSlot);

            //no section is exists
            if (section == null)
            {
                return false;
            }
            //section is exists
            else
            {
                //convert world coord to block slot
                Vector3Int blkslot = CoordToBlockSlot(Coord);

                //case: no block is placed
                if (section.GetBlock(blkslot.x, blkslot.y, blkslot.z) == null)
                {
                    return false;
                }
                //case: a block is placed
                else
                {
                    blkpos = new BlockPosition(Coord);
                    return true;
                }
            }
        }

        public float GetGroundHeight(Vector3 Coord)
        {
            Chunk chk = GetChunk(Coord);
            if (chk == null || Coord.y < 0) return float.MinValue;

            Vector3Int blkslot = CoordToBlockSlot(Coord);

            float res;
            if (chk.GetGroundHeight(blkslot.x, blkslot.z, (int)Coord.y, out res))
                return res;
            else return float.MinValue;
        }
    }







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
        public ChunkInWorld(int x,int y, IWorld _world)
        {
            m_Value = new Vector2Int(
                x >= 0 ?
                    x / _world.Section_Width
                    : x/ _world.Section_Width - 1,
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
        public Vector2Int m_Value;
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
            return new ChunkInWorld(m_Value.x, m_Value.z,_world);
        }
        public SectionInChunk ToSectionInChunk()
        {
            return new SectionInChunk(m_Value.y);
        }

        public Vector3Int Move(Vector3Int offset)
        {
            m_Value += offset;
            return m_Value;
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
        public bool IsInSectionBorder(IWorld _world)
        {
            if (Value.x == _world.Section_Width - 1 || Value.x == 0) return true;
            if (Value.y == _world.Section_Width - 1 || Value.y == 0) return true;
            if (Value.z == _world.Section_Width - 1 || Value.z == 0) return true;
            return false;
        }


        public Vector3Int Move(Vector3Int offset, IWorld _world)
        {
            Value += offset;

            if (m_Value.x > _world.Section_Width) m_Value.x %= _world.Section_Width;
            if (m_Value.y > _world.Section_Height) m_Value.y %= _world.Section_Height;
            if (m_Value.z > _world.Section_Depth) m_Value.z %= _world.Section_Depth;

            if (m_Value.x < 0) m_Value.x = m_Value.x % _world.Section_Width + _world.Section_Width - 1;
            if (m_Value.y < 0) m_Value.y = m_Value.y % _world.Section_Height + _world.Section_Height - 1;
            if (m_Value.z < 0) m_Value.z = m_Value.z % _world.Section_Depth + _world.Section_Depth - 1;

            return Value;
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



    static class GWorldSearcher
    {
        static public Bounds GetBound(Vector3 Coord)
        {
            Bounds Temp = new Bounds();
            Vector3 vt = new Vector3(
                (Coord.x >= 0 ? (int)Coord.x : (int)Coord.x - 1),
                (Coord.y >= 0 ? (int)Coord.y : (int)Coord.y - 1),
                (Coord.z >= 0 ? (int)Coord.z : (int)Coord.z - 1));

            Temp.SetMinMax(vt, vt + Vector3.one);
            return Temp;
        }

        //Search Chunk 
        //-----------------------------------------------------------------------
        static public Chunk GetChunk(Vector3 Coord,IWorld _World)
        {
            return _World.Pool.GetChunk(new ChunkInWorld(Coord, _World));
        }
        static public Chunk GetChunk(ChunkInWorld chunkinworld,IWorld _World)
        {
            return _World.Pool.GetChunk(chunkinworld);
        }

        //Search Section 
        //-----------------------------------------------------------------------
        static public Section GetSection(Vector3 Coord, IWorld _World)
        {         
            //Try get chunk
            Chunk _chunk = GetChunk(Coord, _World);
            if (_chunk == null) return null;

            //Get Section
            return _chunk.GetSection(new SectionInChunk(Coord, _World));
        }
        static public Section GetSection(SectionInWorld Slot,IWorld _World)
        {
            //Try get chunk
            Chunk _chunk = GetChunk(Slot.ToChunkInWorld(_World), _World);
            if (_chunk == null) return null;

            //get Section
            return _chunk.GetSection(Slot.ToSectionInChunk()); 
        }

        //Search Block 
        //-----------------------------------------------------------------------
        static public Block GetBlock(Vector3 Coord, IWorld _World)
        {
            //Try get section
            Section section = GetSection(Coord,_World);
            if (section == null) return null;

            BlockInSection BlockSlot = new BlockInSection(Coord,_World);
            return section.GetBlock(BlockSlot.Value.x, BlockSlot.Value.y, BlockSlot.Value.z);
        }

        static public BlockPosition GetBlockPos(Vector3 Coord)
        {
            return new BlockPosition(Coord);
        }

        static public bool TryGetBlockPos(Vector3 Coord, out BlockPosition blkpos)
        {
            blkpos = default(BlockPosition);

            Vector3Int SectionSlot = CoordToSlot(Coord);

            //Try get section
            Section section = GetSection(SectionSlot);

            //no section is exists
            if (section == null)
            {
                return false;
            }
            //section is exists
            else
            {
                //convert world coord to block slot
                Vector3Int blkslot = CoordToBlockSlot(Coord);

                //case: no block is placed
                if (section.GetBlock(blkslot.x, blkslot.y, blkslot.z) == null)
                {
                    return false;
                }
                //case: a block is placed
                else
                {
                    blkpos = new BlockPosition(Coord);
                    return true;
                }
            }
        }

        static public float GetGroundHeight(Vector3 Coord)
        {
            Chunk chk = GetChunk(Coord);
            if (chk == null || Coord.y < 0) return float.MinValue;

            Vector3Int blkslot = CoordToBlockSlot(Coord);

            float res;
            if (chk.GetGroundHeight(blkslot.x, blkslot.z, (int)Coord.y, out res))
                return res;
            else return float.MinValue;
        }



    }
    
}