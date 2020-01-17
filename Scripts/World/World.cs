using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Noise;


namespace Assets.Scripts.NWorld
{
    public interface IWorld
    {
        ushort Section_Width { get; }
        ushort Section_Height { get; }
        ushort Section_Depth { get; }
        ushort Chunk_Height { get; }
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

        //Public Function
        //--------------------------------------------------------------------
        public Vector3Int CoordToSlot(Vector3 Coord)
        {
            return new Vector3Int(
                Coord.x >= 0 ? (int)Coord.x / m_Section_Width : (int)(Coord.x) / m_Section_Width - 1,
                Coord.y >= 0 ? (int)Coord.y / m_Section_Height : (int)(Coord.y) / m_Section_Height - 1,
                Coord.z >= 0 ? (int)Coord.z / m_Section_Depth : (int)(Coord.z) / m_Section_Depth - 1);
        }
        public Vector3 SlotToCoord(Vector3Int Slot)
        {
            return new Vector3(
                (float)Slot.x * m_Section_Width,
                (float)Slot.y * m_Section_Width,
                (float)Slot.z * m_Section_Width);
        }

        public Vector3Int CoordToBlockSlot(Vector3 Coord)
        {
            return new Vector3Int(
                (Coord.x >= 0 ? (int)Coord.x % m_Section_Width : (int)Coord.x % m_Section_Width + m_Section_Width - 1),
                (Coord.y >= 0 ? (int)Coord.y % m_Section_Height : (int)Coord.y % m_Section_Height + m_Section_Height - 1),
                (Coord.z >= 0 ? (int)Coord.z % m_Section_Depth : (int)Coord.z % m_Section_Depth + m_Section_Depth - 1)
                );
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
            Section _section = _chunk.GetSection(Slot.y);
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

        public void GetBlockPos(Vector3 Coord, out BlockPosition blkpos)
        {
            blkpos = default(BlockPosition);

            Vector3Int SectionSlot = CoordToSlot(Coord);

            //get section
            Section section = GetSection(SectionSlot);

            //convert world coord to block slot
            Vector3Int blkslot = CoordToBlockSlot(Coord);

            blkpos = new BlockPosition(section, blkslot, GetBound(Coord));
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
                    blkpos = new BlockPosition(section, blkslot,GetBound(Coord));
                    return true;
                }
            }
        }

        public float GetGroundHeight(Vector3 Coord)
        {
            Chunk chk = GetChunk(Coord);
            if (chk == null|| Coord.y<0) return float.MinValue;

            Vector3Int blkslot = CoordToBlockSlot(Coord);

            float res;
            if (chk.GetGroundHeight(blkslot.x, blkslot.z,(int)Coord.y, out res))
                return res;
            else return float.MinValue;
        }
    }
}
