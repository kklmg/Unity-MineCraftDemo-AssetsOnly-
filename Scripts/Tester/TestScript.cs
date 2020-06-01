using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Threading;

using UnityEngine;

using Assets.Scripts.NData;
using Assets.Scripts.NMesh;
using Assets.Scripts.NWorld;
using System;

namespace Assets.Scripts.Tester
{
    [System.Serializable]
    public struct TestST
    {
        public int HP;
        public int MP;
        public int STR;
        public int DEF;
        private int AGL;

        public List<int> weapon;
    }




    class TestScript : MonoBehaviour
    {
        //public uint count;
        //private Thread m_thread;
        //private EventWaitHandle m_WaitHandle;

        //public bool ActiveThread;

        //private void Awake()
        //{
        //    m_WaitHandle = new EventWaitHandle(false,EventResetMode.ManualReset);
        //    m_thread = new Thread(thread00);
        //    m_thread.Start();
        //}

        //private void Update()
        //{
        //    if (ActiveThread)
        //    {
        //        if (m_WaitHandle.Set()) Debug.Log("actived");
        //    }

        //    else
        //    {
        //        if (m_WaitHandle.Reset()) Debug.Log("Disactived");
        //    } 
        //}


        //private void OnEnable()
        //{
            
        //}


        //void thread00()
        //{
        //    Debug.Log("start thread ");
        //    while (true)
        //    {
        //        m_WaitHandle.WaitOne();
        //        count++;
        //        //Debug.Log("thread is Running");               
        //    }
        //}

        //private void OnDestroy()
        //{
        //    m_thread.Abort();
        //    Debug.Log("Thread aborted");
        //}
        //private void OnDisable()
        //{
        //    m_WaitHandle.Reset();
        //    //m_thread.Abort();
        //    Debug.Log("Thread aborted");
        //}
        //private void OnApplicationQuit()
        //{
        //    m_thread.Abort();
        //    Debug.Log("Thread aborted");
        //}

    }
}







