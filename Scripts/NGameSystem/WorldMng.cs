using UnityEngine;
using System.Threading;

using Assets.Scripts.NData;
using Assets.Scripts.NWorld;
using Assets.Scripts.NGlobal.ServiceLocator;

namespace Assets.Scripts.NGameSystem
{
    [System.Serializable]
    [RequireComponent(typeof(SaveMng))]
    [RequireComponent(typeof(PlayerMng))]
    class WorldMng : MonoBehaviour, IGameMng
    {
        public GameObject m_WorldPrefab;
        private GameObject m_WorldIns;

        public IWorld WorldService { get; private set; }

        public GameObject WorldIns { get { return m_WorldIns; } }

        public void ApplySettings(GameSetting setting)
        {
            

            return;
        }

        public void InitWorldService()
        {
            m_WorldIns = Instantiate(m_WorldPrefab);

            WorldService = m_WorldIns.GetComponent<World>();
            
            WorldService.Init(GetComponent<SaveMng>().WorldSeed);

            Locator<IWorld>.ProvideService(WorldService);
        }

        public void SpawnWorld()
        {
            ChunkSpawner spawner = WorldService.Entity.GetComponent<ChunkSpawner>();
            int ViewDistance = (int)GetComponent<PlayerMng>().PlayerView;

            spawner.SpawnAt(GetComponent<SaveMng>().PlayerPos, ViewDistance, WorldService);
        }

    }
}


