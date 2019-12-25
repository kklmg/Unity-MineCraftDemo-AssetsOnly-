using UnityEngine;

namespace Assets.Scripts.BehaviorTree
{
    //Behavior Status
    public enum eNodeState
    {
        Failed = 0,
        Suceed = 1,
        Ready,
        Running,
        Completed
    };

    //Base Behavior Node
    public abstract class BevNodeBase : ScriptableObject//,ICloneable
    {
        //Field
        //----------------------------------------------------------------------
        protected bool m_bVisited;
        protected eNodeState m_enNodeState;

        //Property
        //----------------------------------------------------------------------
        public bool Visited { get { return m_bVisited; } }

        //Constructor
        //----------------------------------------------------------------------
        public BevNodeBase()
        {
            Reset();
        }

        //Public Function
        //----------------------------------------------------------------------
        public eNodeState Evaluate()
        {
            if (m_bVisited == false)
                Enter();

            return Tick();
        }

        //virtual Function
        //----------------------------------------------------------------------
        protected virtual void Reset()
        {
            m_bVisited = false;
            m_enNodeState = eNodeState.Ready;
        }
        protected virtual void Enter() { }
        protected virtual void Exit() { }

        //abstract Function
        //----------------------------------------------------------------------
        protected abstract eNodeState Tick();
    }
}