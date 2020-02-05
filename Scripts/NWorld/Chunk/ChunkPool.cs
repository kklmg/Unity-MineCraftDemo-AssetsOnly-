using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.NEvent;
using Assets.Scripts.NCommand;
using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NCache;

namespace Assets.Scripts.NWorld
{
    [RequireComponent(typeof(World))]
    public class ChunkPool : MonoBehaviour
    {
        //Field
        //---------------------------------------------------------------------------
        public GameObject m_Prefab_Chunk;

        [SerializeField]
        private LRUCache<Vector2Int, Chunk> m_Chunks;

        [SerializeField]
        private LinkedList<Com_ChangeBlock> m_BlockChanges = new LinkedList<Com_ChangeBlock>();

        [SerializeField]
        [Range(2,50)]
        private int m_SaveChangeCount = 20; //Block Changes Stroage

        [SerializeField]
        private uint m_MaxCount = 10;

        private IWorld m_refWorld;


        private int m_PlayerView;    //Player's View Distance

        public int test;

        //test
        public double t_timeelapsed;

        //Unity Function
        //---------------------------------------------------------------------------
        private void Awake()
        {
            m_refWorld = GetComponent<IWorld>();
            m_Chunks = new LRUCache<Vector2Int, Chunk>(m_MaxCount);
        }

        private void Start()
        {
            Locator<IEventSubscriber>.GetService().Subscribe(E_Player_Spawned.ID, Handle_PlayerSpawn);
            Locator<IEventSubscriber>.GetService().Subscribe(E_Block_Change.ID, Handle_BlockChange);
            Locator<IEventSubscriber>.GetService().Subscribe(E_Block_Recover.ID, Handle_BlockRecover);
            Locator<IEventSubscriber>.GetService().Subscribe(E_Player_LeaveChunk.ID, SpawnChunk_InPlayerView);
        }

        //public Function
        //---------------------------------------------------------------------------    
        public void Spawn(ChunkInWorld _chunkinworld)
        {
            Chunk TempChunk;
            //Case: The Chunk has already spawned
            if (m_Chunks.TryGetValue(_chunkinworld.Value, out TempChunk))
            {
                ++test;
                return;
            }
                
            //Case: no enough space in pool 
            else if (m_Chunks.IsFull())
            {
                double timestart = Time.deltaTime;
                //Get Least recently used Chunk
                TempChunk = m_Chunks.Pop_LRU();

                //reset chunk
                TempChunk.Init(_chunkinworld);

                TempChunk.transform.localPosition = _chunkinworld.ToCoord3D(m_refWorld);
                TempChunk.transform.name = "Chunk" + "[" + _chunkinworld.Value.x + "]" + "[" + _chunkinworld.Value.y + "]";

                //put to LRUCache
                m_Chunks.Put(_chunkinworld.Value, TempChunk);

                t_timeelapsed = Time.deltaTime - timestart;
            }
            else
            {
                //make a chunk instance
                TempChunk = 
                    Instantiate(m_Prefab_Chunk ,transform).GetComponent<Chunk>();
                TempChunk.transform.localPosition = _chunkinworld.ToCoord3D(m_refWorld);

                //set name
                TempChunk.transform.name = "Chunk" + "[" + _chunkinworld.Value.x + "]" + "[" + _chunkinworld.Value.y + "]";

                //Set Chunk 
                TempChunk.Init(_chunkinworld);

                //put to pool
                m_Chunks.Put(_chunkinworld.Value, TempChunk);
            }    
        }

        public bool SpawnChunk_InPlayerView(IEvent _event)
        {
            //Interpret Event
            E_Player_LeaveChunk LeaveChunk = (_event as E_Player_LeaveChunk);            

            //spawn the chunks in character view
            int left = LeaveChunk.ChunkInWorld.x - LeaveChunk.playerView;
            int top = LeaveChunk.ChunkInWorld.y + LeaveChunk.playerView;
            int right = LeaveChunk.ChunkInWorld.x + LeaveChunk.playerView;
            int bottom = LeaveChunk.ChunkInWorld.y - LeaveChunk.playerView;


            int i, j;
            for (i = left; i < right; ++i)
            {
                for (j = bottom; j < top; ++j)
                {
                    Spawn(new ChunkInWorld(new Vector2Int(i, j), m_refWorld));
                }
            }


            return true;
        }

        public bool Handle_BlockChange(IEvent _event)
        {
            E_Block_Change temp = (_event as E_Block_Change);

            //Execute change request
            temp.Request.Execute();

            //save changes 
            m_BlockChanges.AddLast(temp.Request);

            //if changes space is full,drop the first change
            while (m_BlockChanges.Count >= m_SaveChangeCount)
            {
                m_BlockChanges.RemoveFirst();
            }
            Debug.Log("block has Changed!, cache size: "+ m_BlockChanges.Count);
            return true;
        }

        public bool Handle_BlockRecover(IEvent _event)
        {
            if (m_BlockChanges.Count > 0)
            {
                m_BlockChanges.Last.Value.Undo();
                m_BlockChanges.RemoveLast();

                Debug.Log("block has Recovered!, cache size: " + m_BlockChanges.Count);
                return true;
            }

            else return false;
        }

        public bool Handle_PlayerSpawn(IEvent _event)
        {
            E_Player_Spawned temp = (_event as E_Player_Spawned);

            //Save player's view distance
            m_PlayerView = temp.Player.ViewDistance;

            ChunkInWorld SpawnCenter = new ChunkInWorld(temp.SpawnPos,m_refWorld);

            //calculate area where chunks will be spawned at
            int left = SpawnCenter.x - m_PlayerView;
            int top = SpawnCenter.y - m_PlayerView;
            int right = SpawnCenter.x + m_PlayerView;
            int bottom = SpawnCenter.y + m_PlayerView;


            for (int i = left; i < right; ++i)
            {
                for (int j = top; j < bottom; ++j)
                {
                    Spawn(new ChunkInWorld(new Vector2Int(i, j), m_refWorld));
                }
            }

            return true;
        }

        public Chunk GetChunk(ChunkInWorld _Chunkinworld)
        {
            Chunk _Chunk;
            //Case: The Chunk is already spawned
            if (m_Chunks.TryGetValue(_Chunkinworld.Value, out _Chunk))
                return _Chunk;
            return null;
        }



        //Coroutine
        //--------------------------------------------------------------------------------
        private IEnumerator SpawnChunks_Corou(int left, int top, int right, int bottom)
        {
            int i, j;
            for (i = left; i < right; ++i)
            {
                for (j = top; j < bottom; ++j)
                {
                    Spawn(new ChunkInWorld(new Vector2Int(i, j), m_refWorld));
                }
                yield return null;
            }
        }

        private IEnumerator SpawnChunksHor_Corou(int Hor, int left, int right)
        {
            for (int i = left; i < right; ++i)
            {
                Spawn(new ChunkInWorld(new Vector2Int(i,Hor), m_refWorld));
                yield return null;
            }
        }

        private IEnumerator SpawnChunksVer_Corou(int Ver, int Top, int Bottom)
        {
            for (int i = Bottom; i < Top; ++i)
            {
                Spawn(new ChunkInWorld(new Vector2Int(Ver,i ), m_refWorld));
                yield return null;
            }
        }


    }
}




//LeaveChunk.Offset.x;

//    if (LeaveChunk.Offset.x > 0)
//{
//    IEnumerator CorouV = SpawnChunksVer_Corou(right, top, bottom);
//    StartCoroutine(CorouV);
//}
//else if (LeaveChunk.Offset.x < 0)
//{
//    IEnumerator CorouV = SpawnChunksVer_Corou(left, top, bottom);
//    StartCoroutine(CorouV);
//}

//if (LeaveChunk.Offset.y > 0)
//{
//    IEnumerator CorouH = SpawnChunksHor_Corou(top, left, right);
//    StartCoroutine(CorouH);
//}
//else if (LeaveChunk.Offset.y < 0)
//{
//    IEnumerator CorouH = SpawnChunksHor_Corou(bottom, left, right);
//    StartCoroutine(CorouH);
//}