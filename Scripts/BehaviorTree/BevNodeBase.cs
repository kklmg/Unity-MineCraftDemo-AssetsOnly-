using UnityEngine;
using Assets.Scripts.Pattern;

namespace Assets.Scripts.BehaviorTree
{
    //Base Behavior Node
    //used template method pattern
    //Template - Enter();
    //Template - Tick();
    //Template - Exit();
    //Template - Reset();
    public abstract class BevNodeBase : ScriptableObject //,ICloneable
    {
        //Field
        //----------------------------------------------------------------------
        protected bool m_bVisited;
        protected eRunningState m_enRunningState;
        protected string m_strNodeName;

        //Property
        //----------------------------------------------------------------------
        public bool Visited { get { return m_bVisited; } }
        public eRunningState RunningState{ get { return m_enRunningState; }  }
        public void LogTree()
        {
            //Debug.Log(m_strNodeName);
           // Debug.Log(m_enRunningState);
        }
        //Constructor
        //----------------------------------------------------------------------
        public BevNodeBase()
        {
            Reset();
        }

        //Public Function
        //----------------------------------------------------------------------

        //Run Node
        public eRunningState Evaluate(BevData workData)
        {
            if (m_bVisited == false)
                Enter();

            //run node
            m_enRunningState = Tick(workData);

            if (m_enRunningState == eRunningState.Completed)
                Exit();

            return m_enRunningState;
        }

        //virtual Function     
        //----------------------------------------------------------------------
        protected virtual void Reset()
        {
            m_bVisited = false;
            m_enRunningState = eRunningState.Ready;
        }
        protected virtual void Enter() { }
        protected virtual void Exit() { }

        //abstract Function
        //----------------------------------------------------------------------
        protected abstract eRunningState Tick(BevData workData);
    }
}