using UnityEngine;

using Assets.Scripts.NWorld;
using Assets.Scripts.InputHandler;
using Assets.Scripts.NServiceLocator;

namespace Assets.Scripts.GameManager
{
    class GameManager : MonoBehaviour
    {
        public GameObject m_WorldPrefab;
        private void Awake()
        {
            InitWorrld();
            InitController();
        }
        private void InitWorrld()
        {
            GameObject newWorld = Instantiate(m_WorldPrefab);
            Locator<IWorld>.ProvideService(newWorld.GetComponent<World>());
        }
        private bool InitController()
        {

            Debug.Log(SystemInfo.deviceType);

            switch (SystemInfo.deviceType)  
            {
                case DeviceType.Unknown:
                    break;
                case DeviceType.Handheld:
                    break;
                case DeviceType.Console:
                    break;
                case DeviceType.Desktop:
                    Locator<IController>.ProvideService(new Controller_PC());
                    break;
                default:
                    break;
            }
           
            if (Locator<IController>.GetService() == null)
                Locator<IController>.ProvideService(new Controller_PC());

            return true;
        }
    }
}


