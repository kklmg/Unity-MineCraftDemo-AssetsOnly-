using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Assets.Scripts.NGameSystem;
using Assets.Scripts.NGlobal.Singleton;

namespace Assets.Scripts.NTouch
{
    class ToolBar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public SelectableButton PutButton;
        public SelectableButton SetButton;
        public SelectableButton DestroyButton;
        public Button UndoButton;

        private Picker m_BlockPicker;

        public void Awake()
        {
            m_BlockPicker = MonoSingleton<GameSystem>.Instance.GetPicker();

            PutButton.SetSelectAction(ActivePutButton);
            SetButton.SetSelectAction(ActiveSetButton);
            DestroyButton.SetSelectAction(ActiveDestroyButton);

            PutButton.SetDisSelectAction(DisActiveButton);
            SetButton.SetDisSelectAction(DisActiveButton);
            DestroyButton.SetDisSelectAction(DisActiveButton);

            UndoButton.onClick.AddListener(ClickUndoButton);
        }
        public void Start()
        {
            //Set default tool
            PutButton.OnSelect();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            m_BlockPicker.gameObject.SetActive(false);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            m_BlockPicker.gameObject.SetActive(true);
        }

        public void ActivePutButton()
        {
            SetButton.UnSelect();
            DestroyButton.UnSelect();

            m_BlockPicker.PickerMode = EnPickerMode.ePut;
        }

        public void ActiveSetButton()
        {
            PutButton.UnSelect();
            DestroyButton.UnSelect();

            m_BlockPicker.PickerMode = EnPickerMode.eSet;
        }

        public void ActiveDestroyButton()
        {
            PutButton.UnSelect();
            SetButton.UnSelect();

            m_BlockPicker.PickerMode = EnPickerMode.eDestroy;
        }

        public void DisActiveButton()
        {
            m_BlockPicker.PickerMode = EnPickerMode.eNormal;
        }

        public void ClickUndoButton()
        {
            m_BlockPicker.UndoBlockModify(); 
        }

    }
}
