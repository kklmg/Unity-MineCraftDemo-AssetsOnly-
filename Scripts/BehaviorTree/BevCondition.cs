using UnityEngine;
using Assets.Scripts.Condition;


namespace Assets.Scripts.BehaviorTree
{
    //Abstact Condition Node
    public abstract class BevConditionBase : BevNodeBase
    {
        //Field
        //------------------------------------------------------------
        [SerializeField]
        private bool isTrue;

        //Override Function
        //-------------------------------------------------------------
        protected override eNodeState Tick()
        {
            isTrue = Check();
            if (isTrue)
                return eNodeState.Suceed;
            else
                return eNodeState.Failed;
        }

        //Abstact Function
        protected abstract bool Check();
    };

    //Condition Node construct use Interface
    public class BevCondition_Itf : BevNodeBase
    {
        //Field
        //------------------------------------------------------------
        [SerializeField]
        private ConditionBase m_Con;

        [SerializeField]
        private bool isTrue;

        //Constructor
        //-------------------------------------------------------------
        public BevCondition_Itf(ConditionBase conbase)
        {
            m_Con = conbase;
        }


        //Override Function
        //-------------------------------------------------------------
        protected override eNodeState Tick()
        {
            isTrue = m_Con.Check();
            if (isTrue)
                return eNodeState.Suceed;
            else
                return eNodeState.Failed;
        }
    };
    //Condition Node construct use delegate
    [CreateAssetMenu(menuName = "Bev/Con_delegate")]
    public class BevCondition_Del : BevNodeBase
    {
        //Field
        //------------------------------------------------------------
        [SerializeField]
        private del_Condition m_delCondition;

        [SerializeField]
        private bool isTrue;

        //Constructor
        //-------------------------------------------------------------
        public BevCondition_Del(del_Condition del_con)
        {
            m_delCondition = del_con;
        }


        //Override Function
        //-------------------------------------------------------------
        protected override eNodeState Tick()
        {
            isTrue = m_delCondition();
            if (isTrue)
                return eNodeState.Suceed;
            else
                return eNodeState.Failed;
        }
    };


}
