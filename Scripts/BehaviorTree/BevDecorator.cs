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
        protected override eRunningState Tick(BevData workData)
        {
            eRunningState res;
            res = m_Child.Evaluate(workData);

            if (res == eRunningState.Completed) res = eRunningState.Ready;
            return res;
        }
    };

    //Reverse Decorator
    [CreateAssetMenu(menuName = "Bev/Reverser")]
    class BevReverser : BevDecorator
    {
        protected override eRunningState Tick(BevData workData)
        {
            eRunningState res;

            res = m_Child.Evaluate(workData);
            res = res == eRunningState.Suceed ? eRunningState.Failed : eRunningState.Suceed;

            return res;
        }
    };
}
