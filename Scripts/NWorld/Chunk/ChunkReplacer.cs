using System;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;

using Assets.Scripts.NData;
using Assets.Scripts.NGameSystem;
using Assets.Scripts.NEvent;
using Assets.Scripts.NGlobal.Singleton;
using Assets.Scripts.NGlobal.ServiceLocator;


namespace Assets.Scripts.NWorld
{
    [RequireComponent(typeof(WorldEntity))]
    [RequireComponent(typeof(ChunkMeshBuilder))]
    class ChunkReplacer : MonoBehaviour
    {
        private IWorld m_refWorld;
        private WorldEntity m_WorldEntiry;
        private SaveMng m_SaveMng;
        private PlayerMng m_PlayerMng;

        private ChunkMeshBuilder m_ChunkMeshBuilder;

        [SerializeField]
        private List<Chunk> m_ChunkReplaced;

        private List<ChunkInWorld> m_LocNotInView;

        [SerializeField]
        private Dictionary<Vector2Int, Vector2Int> m_Replace;

        [SerializeField]
        private AreaRect m_ViewArea;

        //Unity Function
        //---------------------------------------------------------------------------
        private void Awake()
        {
            m_WorldEntiry = GetComponent<WorldEntity>();           
            m_PlayerMng = MonoSingleton<GameSystem>.Instance.PlayerMngIns;
            m_SaveMng = MonoSingleton<GameSystem>.Instance.SaveMngIns;

            m_Replace = new Dictionary<Vector2Int, Vector2Int>();
            m_ChunkReplaced = new List<Chunk>();
            m_LocNotInView = new List<ChunkInWorld>();

            m_ChunkMeshBuilder = GetComponent<ChunkMeshBuilder>();
        }

        private void Start()
        {
            m_refWorld = Locator<IWorld>.GetService();

            Locator<IEventSubscriber>.GetService().Subscribe(E_Player_LeaveChunk.ID, Handle_PlayerLeaveChunk);
            Locator<IEventSubscriber>.GetService().Subscribe(E_Player_Spawned.ID, Handle_PlayerSpawn);
        }

        //public Function
        //--------------------------------------------------------------------------- 
        public Chunk Replace(ChunkInWorld bePlace, ChunkInWorld Place)
        {

            //Case: The Chunk has already spawned
            if (m_WorldEntiry.TryGetChunk(bePlace, out Chunk TempChunk))
            {
                m_WorldEntiry.Remove(bePlace);

                //reset chunk
                TempChunk.SetLocation(Place);

                //GTimer.ShowElapsedTimeAndRestart("set loc");

                TempChunk.GenerateBlocks();
                //TempChunk.BuildMeshInstantly();
                //TempChunk.RunBuildMeshCoro();
               

                m_SaveMng.LoadBlock(Place, TempChunk);

                m_ChunkMeshBuilder.AddBuildRequest(TempChunk);

                //GTimer.ShowElapsedTimeAndRestart("Load block");

                //GTimer.RestartTimer();

                //ThreadPool.QueueUserWorkItem(Thread_CreateChunkMesh, TempChunk);

                //Thread thread = new Thread(new ParameterizedThreadStart(Thread_CreateChunkMesh));
                //thread.Start(TempChunk);


                //GTimer.ShowElapsedTimeAndRestart("thread?????????");
                //GTimer.ShowElapsedTimeAndRestart("queue thread");

                //TempChunk.BuildMeshInBackground();

                //put to cache
                m_WorldEntiry.AddChunk(Place, TempChunk);

                return TempChunk;
            }
            else return null;

        }

        //Event Handler
        //--------------------------------------------------------------------------- 
        public bool Handle_PlayerSpawn(IEvent _event)
        {
            E_Player_Spawned temp = (_event as E_Player_Spawned);

            ChunkInWorld SpawnCenter = new ChunkInWorld(temp.SpawnPos, m_refWorld);

            //figure out visiable area 
            m_ViewArea = new AreaRect(SpawnCenter.Value, (int)m_PlayerMng.PlayerView);

            return true;
        }

        public bool Handle_PlayerLeaveChunk(IEvent _event)
        {
            GSW.RestartTimer();
            //Interpret Event
            E_Player_LeaveChunk LeaveChunk = (_event as E_Player_LeaveChunk);

            m_Replace.Clear();
            m_ChunkReplaced.Clear();
            m_LocNotInView.Clear();

            m_ViewArea.Move(LeaveChunk.Offset);
            m_WorldEntiry.GetNotInArea(ref m_ViewArea, m_LocNotInView);

            int i, j, k = 0;
            ChunkInWorld TempLoc;
            for (i = m_ViewArea.left; i <= m_ViewArea.right; ++i)
            {
                for (j = m_ViewArea.bottom; j <= m_ViewArea.top; ++j)
                {

                    TempLoc = new ChunkInWorld(new Vector2Int(i, j), m_refWorld);

                    if (!m_WorldEntiry.Contains(TempLoc))
                    {
                        m_ChunkReplaced.Add(Replace(m_LocNotInView[k++], TempLoc));
                    }
                }
            }
            GSW.ShowElapsedTimeAndRestart("replace all");
            return true;
        }
    }
}
