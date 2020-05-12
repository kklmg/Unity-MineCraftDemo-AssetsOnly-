using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Scripts.NWorld;
using Assets.Scripts.NGlobal.Singleton;
using Assets.Scripts.NGlobal.ServiceLocator;

namespace Assets.Scripts.NTouch
{
  
    class BlockBar : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {
        public Color SelectedColor;
        public Color UnSelectedColor;

        public BlockButton DefaultButton;

        public BlockButton CurSelectedButton { set; get; }

        private IWorld m_refWorld;


        private void Start()
        {
            m_refWorld = Locator<IWorld>.GetService();
            BlockPalette Palette = m_refWorld.BlkPalette;
            var buttons = GetComponentsInChildren<BlockButton>();

            byte i = 1;
            foreach (var bt in buttons)
            {
                if (i >= Palette.Count) break;
                bt.SetBlock(Palette[i],i);
                ++i;
            }

            //set default selected
            DefaultButton.OnSelect();
        }
        

        public void OnPointerEnter(PointerEventData eventData)
        {
            MonoSingleton<Picker>.Instance.gameObject.SetActive(false);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            MonoSingleton<Picker>.Instance.gameObject.SetActive(true);
        }
    }
}
