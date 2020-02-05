using UnityEngine;

using Assets.Scripts.NWorld;
using Assets.Scripts.NGlobal.Singleton;
using Assets.Scripts.NGlobal.ServiceLocator;

namespace Assets.Scripts.NGameMng
{
    class WorldMng : MonoSingleton<WorldMng>
    {
        public GameObject m_WorldPrefab;

        private GameObject m_WorldIns;

        public GameObject WorldIns { get { return m_WorldIns; } }

        private void Awake()
        {
            _Init();
        }

        private void _Init()
        {
            m_WorldIns = Instantiate(m_WorldPrefab);
            Locator<IWorld>.ProvideService(m_WorldIns.GetComponent<World>());
        }
    }
}


