using UnityEngine;
using System.Threading;

using Assets.Scripts.NData;
using Assets.Scripts.NWorld;
using Assets.Scripts.NGlobal.ServiceLocator;

namespace Assets.Scripts.NGameSystem
{
    [RequireComponent(typeof(SaveMng))]
    [RequireComponent(typeof(PlayerMng))]
    class WorldMng : MonoBehaviour
    {
        public GameObject m_WorldPrefab;
        private GameObject m_WorldIns;

        public IWorld WorldService { get; private set; }

        public GameObject WorldIns { get { return m_WorldIns; } }

        public void InitWorldService()
        {
            int workerThreads;
            int portThreads;

            ThreadPool.GetMaxThreads(out workerThreads, out portThreads);
            Debug.Log("workerThreads" + workerThreads);
            Debug.Log("Maximum completion port threads" + portThreads);

            ThreadPool.GetAvailableThreads(out workerThreads, out portThreads);
            Debug.Log("available workerThreads" + workerThreads);
            Debug.Log("available Maximum completion port threads" + portThreads);


            ThreadPool.SetMaxThreads(100, 100);

            m_WorldIns = Instantiate(m_WorldPrefab);

            WorldService = m_WorldIns.GetComponent<World>();
            
            WorldService.Init(GetComponent<SaveMng>().LoadedFile.WorldSeed);

            Locator<IWorld>.ProvideService(WorldService);
        }

        public void SpawnWorld()
        {
            ChunkSpawner spawner = WorldService.Entity.GetComponent<ChunkSpawner>();

            GameSave SaveData = GetComponent<SaveMng>().LoadedFile;
            int ViewDistance = (int)GetComponent<PlayerMng>().PlayerView;

            spawner.SpawnAt(SaveData.PlayerPos, ViewDistance, WorldService);
        }

    }
}


