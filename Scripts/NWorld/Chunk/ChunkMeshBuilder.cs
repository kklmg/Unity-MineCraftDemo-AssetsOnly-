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
    class ChunkMeshBuilder : MonoBehaviour, IDisposable
    {
        private Thread m_TMeshBuilder;
        private object m_Locker;
        private Queue<Chunk> m_BuildRequests = new Queue<Chunk>();
        private SaveMng m_SaveMng;

        private Chunk m_ChunkCache;

        private void Awake()
        {
            m_SaveMng = MonoSingleton<GameSystem>.Instance.SaveMngIns;

            m_TMeshBuilder = new Thread(new ThreadStart(Thread_BuildMesh));
            m_TMeshBuilder.Priority = System.Threading.ThreadPriority.Highest;
            m_Locker = new object();
        }

        private void Start()
        {
            m_TMeshBuilder.Start();
        }

        public void AddBuildRequest(Chunk _chunk)
        {
            m_BuildRequests.Enqueue(_chunk);
        }

        void Thread_BuildMesh()
        {
            while (true)
            {
                if (m_BuildRequests.Count != 0)
                {
                    //lock (m_Locker)
                    {
                        m_ChunkCache = m_BuildRequests.Dequeue();
                    }
                    //m_ChunkCache.GenerateBlocks();
                    m_ChunkCache.BuildMesh();
                }
            }
        }
        

        public void Dispose()
        {
            m_TMeshBuilder.Abort();
            Debug.Log("aborted");
        }
    }
}
