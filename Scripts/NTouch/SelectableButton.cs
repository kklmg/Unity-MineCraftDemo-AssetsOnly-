using System;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Assets.Scripts.NTouch
{
    [RequireComponent(typeof(Image))]
    class SelectableButton : MonoBehaviour, IPointerClickHandler
    {
        public Color NormalColor;
        public Color SelectedColor;

        private Image m_Image;

        private Action m_SelectAction;
        private Action m_DisselectAction;

        public bool IsSelected { private set; get; }

        public void Awake()
        {
            m_Image = GetComponent<Image>();
            m_Image.color = NormalColor;
        }

        public void SetSelectAction(Action action)
        {
            m_SelectAction = action;
        }
        public void SetDisSelectAction(Action action)
        {
            m_DisselectAction = action;
        }

        public void OnSelect()
        {
            m_Image.color = SelectedColor;
            m_SelectAction();

            IsSelected = true;
            Debug.Log("selected");
        }

        public void UnSelect()
        {
            m_Image.color = NormalColor;
            m_DisselectAction();

            IsSelected = false;
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            if (IsSelected)
            {
                UnSelect();
            }
            else
            {
                OnSelect();
            } 
        }
    }
}
