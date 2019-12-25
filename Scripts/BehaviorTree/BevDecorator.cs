using UnityEngine;

namespace Assets.Scripts.BehaviorTree
{
    //Base Decorate Node
    abstract class BevDecorator : BevNodeBase
    {
        //Child
        [SerializeField]
        protected BevNodeBase m_Child;

        public void setChild(BevNodeBase Child)
        {
            m_Child = Child;
        }
    };

    //Repeat Decorator
    [CreateAssetMenu(menuName = "Bev/Repeator")]
    class BevRepeator : BevDecorator
    {
        protected override eNodeState Tick()
        {
            eNodeState res;
            res = m_Child.Evaluate();

            if (res == eNodeState.Completed) res = eNodeState.Ready;
            return res;
        }
    };

    //Reverse Decorator
    [CreateAssetMenu(menuName = "Bev/Reverser")]
    class BevReverser : BevDecorator
    {
        protected override eNodeState Tick()
        {
            eNodeState res;

            res = m_Child.Evaluate();
            res = res == eNodeState.Suceed ? eNodeState.Failed : eNodeState.Suceed;

            return res;
        }
    };
}
