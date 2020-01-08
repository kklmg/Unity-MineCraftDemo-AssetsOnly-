using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Pattern;

namespace Assets.Scripts.WorldComponent
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

        //Biomes
        [SerializeField]
        private List<Biome> m_Bimoes;

        //[SerializeField]
        //private Dictionary<Vector2Int, Section> m_SectionMap;

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

        public List<Block> BlockList { get { return m_listBlocks; } }
        public TextureSheet TexSheet { get { return m_TextureSheet; } }
        public List<Biome> Biomes { get { return m_Bimoes; } }

        //unity function
        //--------------------------------------------------------------------
        private void Awake()
        {
            m_refPool = GetComponent<ChunkPool>();
            // m_SectionMap = new Dictionary<Vector3Int, Section>();
        }


        //Public Function
        //--------------------------------------------------------------------
        public Vector3Int CoordToSlot(Vector3 pos)
        {
            return new Vector3Int(
                (int)pos.x / m_Section_Width,
                (int)pos.y / m_Section_Height,
                (int)pos.z / m_Section_Depth);
        }
        public Vector3 SlotToCoord(Vector3Int pos)
        {
            return new Vector3(
                (int)pos.x * m_Section_Width,
                (int)pos.y * m_Section_Height,
                (int)pos.z * m_Section_Depth);
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

            return section.GetBlock(
                (int)Coord.x % m_Section_Width,
                (int)Coord.y % m_Section_Height,
                (int)Coord.z % m_Section_Depth);
        }
        public Block GetBlock_Ray(Ray ray)
        {
            Vector3 check;
            Block block;
            int distance = 6;

            do
            {
                check = ray.origin + ray.direction;
                block = GetBlock(check);
                --distance;
            }
            while (block != null && distance > 0);

            return block;
        }


        public class PlayerSelection
        {
            public Camera m_Camrea;


        }
        //public void RegisterSection(Vector3Int SectionSlot, Section _section)
        //{
        //    m_SectionMap.Add(SectionSlot, _section);
        //}
        //public float GetGroundHeight(Vector3 Coord)
        //{
        //    Vector3Int Slot = CoordToSlot(Coord);
        //    Chunk cur = m_refPool.GetChunk(new Vector2Int(Slot.x, Slot.z));



        //    if (m_SectionMap.TryGetValue(Slot, out receiver))
        //    {
        //        return receiver;
        //    }
        //}



        public class PosData
        {
            World m_refWorld = null;
            Vector3Int m_Slot = Vector3Int.zero;            
            Chunk m_Chunk = null;
            Section m_Section = null;
            Vector3Int m_SlotInSection = Vector3Int.zero;
            Block m_Block = null;

            public PosData(Vector3 Coord)
            {
                m_refWorld = Locator<World>.GetService();
                Update(Coord);
            }
            void Update(Vector3 Coord)
            {
                if (m_refWorld == null)
                {
                    m_Slot = Vector3Int.zero;
                    m_Chunk = null;
                    m_Section = null;
                    m_Block = null;
                    return;
                }
                //update slot
                m_Slot = m_refWorld.CoordToSlot(Coord);
                m_SlotInSection = new Vector3Int(
                    (int)Coord.x % m_refWorld.Section_Width,
                    (int)Coord.y % m_refWorld.Section_Height,
                    (int)Coord.z % m_refWorld.Section_Depth);

                //update chunk
                m_Chunk = m_refWorld.m_refPool.GetChunk(new Vector2Int(m_Slot.x, m_Slot.z));
                if (m_Chunk == null)
                {
                    m_Section = null;
                    m_Block = null;
                    return;
                }

                //update section
                m_Section = m_Chunk.GetSection(m_Slot.y);
                if (m_Section == null)
                {              
                    m_Block = null;
                    return;
                }
                //update block
                m_Section.GetBlock(m_SlotInSection.x, m_SlotInSection.y, m_SlotInSection.z);
            }    
        }

    }
}
