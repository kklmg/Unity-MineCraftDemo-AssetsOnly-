using UnityEngine;
using Assets.Scripts.Pattern;
using Assets.Scripts.WorldComponent;
using Assets.Scripts.InputHandler;
using Assets.Scripts.EventManager;

namespace Assets.Scripts.GameManager
{
    class GameManager : MonoBehaviour
    {
        public GameObject m_WorldPrefab;
        private void Awake()
        {
            InitWorrld();
            Debug.Log("tell me why?");
            InitController();
        }
        private void InitWorrld()
        {
            GameObject newWorld = Instantiate(m_WorldPrefab);
            Debug.Log(newWorld.GetComponent<World>() + "world");


            Locator<World>.ProvideService(newWorld.GetComponent<World>());

            Debug.Log(Locator<IWorld>.GetService()+"service");
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


