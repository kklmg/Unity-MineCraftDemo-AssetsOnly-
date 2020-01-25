using UnityEngine;

using Assets.Scripts.NWorld;
using Assets.Scripts.NGlobal.ServiceLocator;

namespace Assets.Scripts.NGameManager
{
    class WorldManager : MonoBehaviour
    {
        public GameObject m_WorldPrefab;
        private void Awake()
        {
            _Init();
        }
        private void _Init()
        {
            GameObject newWorld = Instantiate(m_WorldPrefab);
            Locator<IWorld>.ProvideService(newWorld.GetComponent<World>());
        }
    }
}


