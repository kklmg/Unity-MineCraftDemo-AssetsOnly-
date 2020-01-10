using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.EventManager;
using Assets.Scripts.Pattern;
using Assets.Scripts.CharacterSpace;

namespace Assets.Scripts.WorldComponent
{
    [RequireComponent(typeof(World))]
    public class ChunkPool : MonoBehaviour
    {
        //Field
        //---------------------------------------------------------------------------
        [SerializeField]
        private Vector3Int m_PreSlot;

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
            Spawn(0, 0);
            Spawn(0, 1);
            Spawn(1, 0);
            Spawn(1, 1);
            Locator<IEventSubscriber>.GetService().Subscribe(E_Cha_Moved.ID, SpawnChunk_NearPlayer);
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
                Go.transform.transform.position = m_refWorld.SlotToCoord(new Vector3Int(slot_x, 0, slot_z));

                _Chunk = Go.AddComponent<Chunk>();
                _Chunk.Init(slot_x, slot_z, this.transform,
                    m_refWorld.Biomes[(int)(m_refWorld.Seed % m_refWorld.Biomes.Count)]);// rand biome

                //put to pool
                m_DicChunks.Add(new Vector2Int(slot_x, slot_z), _Chunk);
            }
            //Case: No Enough Space in Pool
            else return;
        }
        public bool SpawnChunk_NearPlayer(IEvent _event)
        {
            Character Cha = (_event as E_Cha_Moved).Cha;
            Vector3Int CurSlot = m_refWorld.CoordToSlot(Cha.transform.position);

            if (m_PreSlot == CurSlot) return false;
            Debug.Log(CurSlot);

            byte PlayerView = Cha.ViewDistance;

            int left, bottom;
            Spawn(CurSlot.x, CurSlot.z);
            for (left = CurSlot.x - PlayerView; left < CurSlot.x + PlayerView; ++left)
            {
                for (bottom = CurSlot.z - PlayerView; bottom < CurSlot.z + PlayerView; ++bottom)
                {
                    Spawn(left, bottom);
                }
            }
            m_PreSlot = CurSlot;
            return true;
        }

        public Chunk GetChunk(Vector2Int slot)
        {
            Chunk _Chunk;
            //Case: The Chunk is already spawned
            if (m_DicChunks.TryGetValue(slot, out _Chunk))
                return _Chunk;
            return null;
        }
    }
}
