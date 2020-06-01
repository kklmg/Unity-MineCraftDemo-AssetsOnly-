using System;
using System.Collections.Generic;
using System.Threading;


using UnityEngine;

using Assets.Scripts.NGameSystem;
using Assets.Scripts.NGlobal.Singleton;

namespace Assets.Scripts.NWorld
{
    class ChunkMeshBuilder : MonoBehaviour
    {
        [SerializeField]
        private System.Threading.ThreadPriority m_BuildPriority;

        private Thread m_TMeshBuilder;
        private object m_Locker;
        private EventWaitHandle m_WaitHandle;

        //chunks need to build
        private Queue<Chunk> m_BuildRequests = new Queue<Chunk>();    

        private Chunk m_ChunkCache;

        private bool IsThreadInvoked = false;

        private void Awake()
        {
            //make building Thread  
            m_TMeshBuilder = new Thread(new ThreadStart(Thread_BuildMesh));
            m_TMeshBuilder.Priority = m_BuildPriority;
            m_Locker = new object();

            m_WaitHandle = new EventWaitHandle(true,EventResetMode.ManualReset);
            m_WaitHandle.Set();
        }

        private void Start()
        {
            m_TMeshBuilder.Start();
        }


        public void InvokeThread()
        {
            if (IsThreadInvoked == false)
            {
                m_WaitHandle.Set();
                IsThreadInvoked = true;
            }
        }

        public void PauseThread()
        {
            if (IsThreadInvoked)
            {
                if (m_BuildRequests.Count == 0)
                {
                    m_WaitHandle.Reset();
                    IsThreadInvoked = false;
                }
            }
        }

        public void RequestBuildChunk(Chunk _chunk)
        {           
            lock (m_Locker)
            {
                m_BuildRequests.Enqueue(_chunk);
                InvokeThread();
            }
        }

        void Thread_BuildMesh()
        {
            while (true)
            {
                m_WaitHandle.WaitOne();

                if (m_BuildRequests.Count != 0)
                {
                    lock (m_Locker)
                    {
                        m_ChunkCache = m_BuildRequests.Dequeue();
                    }
                    m_ChunkCache.BuildMesh();
                }
                else
                {
                    lock (m_Locker)
                    {
                        PauseThread();
                    }
                }
            }
        }


        private void OnApplicationQuit()
        {
            m_TMeshBuilder.Abort();
        }

        private void OnDestroy()
        {
            m_TMeshBuilder.Abort();
            Debug.Log("Mesh Building Thread aborted");
        }
    }
}
