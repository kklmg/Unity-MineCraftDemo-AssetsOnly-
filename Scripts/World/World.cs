using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.World
{
    public class World : MonoBehaviour
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
        private Dictionary<Vector3Int, Section> m_SectionMap;

        [SerializeField]
        private List<Block> m_listBlocks;

        [SerializeField]
        private TextureSheet m_TextureSheet;

        public Transform player;

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
            m_SectionMap = new Dictionary<Vector3Int, Section>();
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

        public void RegisterSection(Vector3Int slot,Section _section)
        {
            m_SectionMap.Add(slot, _section);
        }
      
        public Section GetSection(Vector3Int Slot)
        {
            Section receiver;
            if (m_SectionMap.TryGetValue(Slot, out receiver))
            {
                return receiver;
            }
            else return null;
        }
    }
}
