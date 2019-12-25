using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.BehaviorTree
{
    //Base Composite Node
    public abstract class BevComposite : BevNodeBase
    {
        //Field
        //----------------------------------------------------------------       
        [SerializeField]
        protected List<BevNodeBase> m_listNodes = new List<BevNodeBase>(); //Child node

        //Property
        //----------------------------------------------------------------       
        protected List<BevNodeBase> Children { get { return m_listNodes; } }

        //public Function
        //-----------------------------------------------------------------
        public void AddChild(BevNodeBase child)
        {
            m_listNodes.Add(child);
        }
    }

    //Sequence Node
    [CreateAssetMenu(menuName = "Bev/Sequence")]
    class BevSequence : BevComposite
    {
        protected override eNodeState Tick()
        {
            eNodeState res;
            foreach (var node in m_listNodes)
            {
                res = node.Evaluate();
                if (res == eNodeState.Running || res == eNodeState.Failed) return eNodeState.Failed;
            }
            return eNodeState.Failed;
        }
    };

    //Selector Node
    [CreateAssetMenu(menuName = "Bev/Selector")]
    class BevSelector : BevComposite
    {
        protected override eNodeState Tick()
        {
            eNodeState res;
            foreach (var node in m_listNodes)
            {
                res = node.Evaluate();
                if (res == eNodeState.Running || res == eNodeState.Suceed) return res;
            }
            return eNodeState.Failed;
        }
    }


    //Parallel Node
    [CreateAssetMenu(menuName = "Bev/Parall")]
    class BevParallel : BevComposite
    {
        protected override eNodeState Tick()
        {
            eNodeState res = eNodeState.Ready;
            foreach (var node in m_listNodes)
            {
                res = node.Evaluate();
            }
            return res;
        }
    };

    //Random Selector
    [CreateAssetMenu(menuName = "Bev/RandSelector")]
    class BevRandSelector : BevComposite
    {
        [SerializeField]
        private int m_nRandFigure;
        protected override eNodeState Tick()
        {
            eNodeState res = m_listNodes[m_nRandFigure].Evaluate();
            return res;
        }
    };
}
