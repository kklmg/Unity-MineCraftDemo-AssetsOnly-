using UnityEngine;
using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NGlobal.Singleton;
using Assets.Scripts.NInput;

namespace Assets.Scripts.NGameMng
{
    class InputMng : MonoSingleton<InputMng>
    {
        private void Awake()
        {
            _Init();
        }

        private bool _Init()
        {
            //Check Device Type        
            switch (SystemInfo.deviceType)
            {
                case DeviceType.Unknown:
                    break;
                case DeviceType.Handheld:
                    break;
                case DeviceType.Console:
                    break;
                case DeviceType.Desktop:
                    Locator<IController>.ProvideService(new Control_PC());
                    break;
                default:
                    break;
            }

            if (Locator<IController>.GetService() == null)
                Locator<IController>.ProvideService(new Control_PC());

            return true;
        }
    }
}
