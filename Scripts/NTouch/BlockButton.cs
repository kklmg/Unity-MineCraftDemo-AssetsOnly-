using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.NWorld;
using Assets.Scripts.NGlobal.Singleton;

namespace Assets.Scripts.NTouch
{
    [RequireComponent(typeof(Button))]
    class BlockButton : MonoBehaviour
    {
        [SerializeField]
        private Image m_Icon;

        [SerializeField]
        private IBlock m_block;
        
        public byte BlockID { private set; get; }

        public bool IsSelected { private set; get; }

        private void Awake()
        { 
            GetComponent<Button>().onClick.AddListener(Click);   
        }

        public void SetBlock(IBlock block,byte id)
        {
            BlockID = id;
            m_block = block;
            m_Icon.sprite = m_block.Icon;
        }

        private void Click()
        {
            if (IsSelected == true)
                return;
            else
                OnSelect();
        }

        public void OnSelect()
        {
            IsSelected = true;

            Picker cursor = MonoSingleton<Picker>.Instance;
            BlockBar bar = GetComponentInParent<BlockBar>();

            GetComponent<Image>().color = bar.SelectedColor;

            if (bar.CurSelectedButton != null)
            {
                bar.CurSelectedButton.UnSelect();
            }
            bar.CurSelectedButton = this;

            //load block type
            cursor.BlockSelected = BlockID;
        }

        public void UnSelect()
        {
            IsSelected = false;
            GetComponent<Image>().color = GetComponentInParent<BlockBar>().UnSelectedColor;
        }
    }
}
