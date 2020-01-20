using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.NEvent;
using Assets.Scripts.NEvent.Impl;
using Assets.Scripts.NServiceLocator;

namespace Assets.Scripts.NWorld
{
    [RequireComponent(typeof(World))]
    public class ChunkPool : MonoBehaviour
    {
        //Field
        //---------------------------------------------------------------------------
        [SerializeField]
        private ChunkInWorld m_PreSlot;

        [SerializeField]
        private Dictionary<Vector2Int, Chunk> m_DicChunks = new Dictionary<Vector2Int, Chunk>();

        [SerializeField]
        private int m_MaxCount = 10;
        private World m_refWorld;

        //Unity Function
        //---------------------------------------------------------------------------
        private void Awake()
        {
            m_refWorld = GetComponent<World>();
            //m_DicChunks = new Dictionary<Vector2Int, Chunk>();
        }
        private void Start()
        {
            Spawn(new ChunkInWorld(0,0,m_refWorld));
            //Spawn(new ChunkInWorld(0, -1, m_refWorld));
            //Spawn(new ChunkInWorld(-1, 0, m_refWorld));
            //Spawn(new ChunkInWorld(-1, -1, m_refWorld));

            Locator<IEventSubscriber>.GetService().Subscribe(E_Block_Change.ID, HandleChunkEvent);
            Locator<IEventSubscriber>.GetService().Subscribe(E_Cha_Moved.ID, SpawnChunk_NearPlayer);
        }

        //public Function
        //---------------------------------------------------------------------------    
        public void Spawn(ChunkInWorld slot)
        {
            Chunk _Chunk;
            //Case: The Chunk has already spawned
            if (m_DicChunks.TryGetValue(slot.Value, out _Chunk))
                return;
            //Case: there is enough space in pool 
            else if (m_DicChunks.Count < m_MaxCount)
            {
                GameObject Go = new GameObject("Chunk" + "[" + slot.Value.x + "]" + "[" + slot.Value.y + "]");
                Go.transform.SetParent(transform);
                Go.transform.transform.position = slot.ToCoord3D(m_refWorld);

                _Chunk = Go.AddComponent<Chunk>();
                _Chunk.Init(slot, this.transform,
                    m_refWorld.Biomes[(int)(m_refWorld.Seed % m_refWorld.Biomes.Count)]);// rand biome

                //put to pool
                m_DicChunks.Add(slot.Value, _Chunk);
            }
            //Case: No Enough Space in Pool
            else return;
        }
        public bool SpawnChunk_NearPlayer(IEvent _event)
        {
            //Interpret Event
            Character Cha = (_event as E_Cha_Moved).Cha;

            //get the chunk position Character located 
            ChunkInWorld CurSlot = new ChunkInWorld(Cha.transform.position,m_refWorld);

            //case: the chunk position Character located has not changed
            if (m_PreSlot.Value == CurSlot.Value) return false;

            byte PlayerView = Cha.ViewDistance;

            //spawn the chunks in character view
            Spawn(CurSlot);

            int left, bottom;
            for (left = CurSlot.Value.x - PlayerView; left < CurSlot.Value.x + PlayerView; ++left)
            {
                for (bottom = CurSlot.Value.y - PlayerView; bottom < CurSlot.Value.y + PlayerView; ++bottom)
                {
                    Spawn(new ChunkInWorld(left, bottom,m_refWorld));
                }
            }
            m_PreSlot = CurSlot;
            return true;
        }

        public bool HandleChunkEvent(IEvent _event)
        {
            E_Block_Change command = (_event as E_Block_Change);
            command.Change.Execute();

            return true;
        }

        public Chunk GetChunk(ChunkInWorld slot)
        {
            Chunk _Chunk;
            //Case: The Chunk is already spawned
            if (m_DicChunks.TryGetValue(slot.Value, out _Chunk))
                return _Chunk;
            return null;
        }

    }
}
