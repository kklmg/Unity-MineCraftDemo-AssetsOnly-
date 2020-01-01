using System.Collections.Generic;

using UnityEngine;
using Assets.Scripts.Pattern;

namespace Assets.Scripts.WorldComponent
{
    public interface IChunkPool
    {
        void Spawn(int slot_x, int slot_z);

        void CreateVisibleChunk();
    }

    [RequireComponent(typeof(World))]
    public class ChunkPool : MonoBehaviour,IChunkPool
    {
        //Field
        //---------------------------------------------------------------------------
        [SerializeField]
        private Character m_refPlayer;
        private Vector3Int m_PreSlot;

        [SerializeField]
        private Dictionary<Vector2Int, Chunk> m_DicChunks;

        [SerializeField]
        private uint m_WorldSeed;

        [SerializeField]
        private int m_MaxCount = 10;
        private World m_refWorld;

        //Unity Function
        //---------------------------------------------------------------------------
        private void Awake()
        {
            m_refWorld = GetComponent<World>();
            m_DicChunks = new Dictionary<Vector2Int, Chunk>();
            CreateVisibleChunk();
        }

        private void Update()
        {
            CreateVisibleChunk();
        }

        //public Function
        //---------------------------------------------------------------------------
        public void Spawn(int slot_x, int slot_z)
        {
            Chunk _Chunk;
            //Case: The Chunk is already spawned
            if (m_DicChunks.TryGetValue(new Vector2Int(slot_x, slot_z), out _Chunk))
                return;
            //Case: Enough space in pool 
            else if (m_DicChunks.Count < m_MaxCount)
            {
                GameObject Go = new GameObject("Chunk" + "[" + slot_x + "]" + "[" + slot_z + "]");
                Go.transform.SetParent(transform);
                Go.transform.transform.position = m_refWorld.SectionSlotToCoord(new Vector3Int(slot_x,0,slot_z));
                
                _Chunk = Go.AddComponent<Chunk>();
                _Chunk.Init(slot_x, slot_z, this.transform, m_refWorld.Biomes[0]);

                //put to pool
                m_DicChunks.Add(new Vector2Int(slot_x, slot_z), _Chunk);
            }
            //Case: No Space in Pool
            else return;
        }
        public void CreateVisibleChunk()
        {
            Vector3Int CurSlot = m_refPlayer.PlayerSlot;
            if (m_PreSlot == CurSlot) return;
        
            byte PlayerView = m_refPlayer.ViewDistance;

            int left, bottom;
            Spawn(CurSlot.x, CurSlot.z);
            for (left = CurSlot.x - PlayerView; left < CurSlot.x+PlayerView ; ++left)
            {
                for (bottom = CurSlot.z - PlayerView; bottom < CurSlot.z+PlayerView; ++bottom)
                {
                    Spawn(left, bottom);                    
                }
            }
            m_PreSlot = CurSlot;
        }
    }
}
