using Assets.Scripts.BehaviorTree;
using UnityEngine;

namespace Assets.Scripts.InputHandler
{
    public abstract class InputNodeBase : BevConditionBase
    {
        [SerializeField]
        protected string m_strKey;

        public InputNodeBase(string key)
        {
            m_strKey = key;
        }
    }

    //Input Axis 
    [CreateAssetMenu(menuName = "Condition/Input/Axis")]
    public class ConInputAxis : InputNodeBase
    {
        ConInputAxis(string key) : base(key) { }

        protected override bool Check()
        {
            float axis = Input.GetAxis(m_strKey);
            return !Mathf.Approximately(axis, 0);
        }
    }

    //Input Button Up
    [CreateAssetMenu(menuName = "Condition/Input/ButtonUp")]
    public class ConButtonUp : InputNodeBase
    {
        public ConButtonUp(string key) : base(key) { }

        protected override bool Check()
        {
            return Input.GetButtonUp(m_strKey);
        }
    }


    //Input Button Down
    [CreateAssetMenu(menuName = "Condition/Input/ButtonDown")]
    public class ConButtonDown : InputNodeBase
    {
        public ConButtonDown(string key) : base(key) { }
        protected override bool Check()
        {
            return Input.GetButtonDown(m_strKey);
        }
    }


    //Input Button Press
    [CreateAssetMenu(menuName = "Condition/Input/ButtonPress")]
    public class ConButtonPress : InputNodeBase
    {
        ConButtonPress(string key) : base(key) { }
        protected override bool Check()
        {
            return Input.GetButton(m_strKey);
        }
    }
}
