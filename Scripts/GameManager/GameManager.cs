using UnityEngine;
using Assets.Scripts.Pattern;
using Assets.Scripts.WorldComponent;
using Assets.Scripts.InputHandler;
using Assets.Scripts.EventManager;

namespace Assets.Scripts.GameManager
{
    class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            //InitWorrld();
            InitController();
        }
        private void InitWorrld()
        {
            GameObject MyWorld = new GameObject("World");
            IWorld refWorld = MyWorld.AddComponent<World>();
            ServiceLocator<IWorld>.ProvideService(refWorld);
        }
        private bool InitController()
        {
            switch (SystemInfo.deviceType)  
            {
                case DeviceType.Unknown:
                    break;
                case DeviceType.Handheld:
                    break;
                case DeviceType.Console:
                    break;
                case DeviceType.Desktop:
                    ServiceLocator<IController>.ProvideService(new Controller_PC());
                    break;
                default:
                    break;
            }

            if (ServiceLocator<IController>.GetService() == null) return false;

            return true;
        }
    }
}


