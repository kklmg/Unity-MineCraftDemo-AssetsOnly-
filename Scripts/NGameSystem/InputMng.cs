using UnityEngine;

using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NTouch;
using Assets.Scripts.NInput;

namespace Assets.Scripts.NGameSystem
{
    class InputMng : MonoBehaviour
    {
        public Canvas prefab_Canvas;
        public GameObject Prefab_Controller;

        public Picker BlockPicker { private set; get; }
        private Character m_player;

        public bool InitInputService()
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

        public void InitController()
        {
            BlockPicker = 
                Instantiate(Prefab_Controller).GetComponent<Picker>();

            Debug.Log("Init picker");
        }

        public void InitInteraction()
        {
            Instantiate(prefab_Canvas);
        }
    }
}
