using UnityEngine;
using Assets.Scripts.WorldComponent;
using Assets.Scripts.Pattern;

namespace Assets.Scripts.Condition
{
    public abstract class Con_TF : ConditionBase
    {
        protected Transform m_trans;
        public Con_TF(Transform trans)
        {
            m_trans = trans;
        }       
    }

    
    public class Con_Blocked_left : Con_TF
    {
        public Con_Blocked_left(Transform trans) : base(trans) { }
        public override bool Check()
        {
            Block target = MonoSingleton<World>.Instance.GetBlock(m_trans.position + Vector3.left);
            if (target == null) return false;
            return target.IsSolid(eDirection.right);
        }
    }
    public class Con_Blocked_Right : Con_TF
    {
        public Con_Blocked_Right(Transform trans) : base(trans) { }
        public override bool Check()
        {
            Block target = MonoSingleton<World>.Instance.GetBlock(m_trans.position + Vector3.right);
            if (target == null) return false;
            return target.IsSolid(eDirection.left);
        }
    }
    public class Con_Blocked_Front : Con_TF
    {
        public Con_Blocked_Front(Transform trans) : base(trans) { }
        public override bool Check()
        {
            Block target = MonoSingleton<World>.Instance.GetBlock(m_trans.position + Vector3.forward);
            if (target == null) return false;
            return target.IsSolid(eDirection.backward);
        }
    }
    public class Con_Blocked_Back : Con_TF
    {
        public Con_Blocked_Back(Transform trans) : base(trans) { }
        public override bool Check()
        {
            Block target = MonoSingleton<World>.Instance.GetBlock(m_trans.position + Vector3.back);
            if (target == null) return false;
            return target.IsSolid(eDirection.forward);
        }
    }
    public class Con_Blocked_Up : Con_TF
    {
        public Con_Blocked_Up(Transform trans) : base(trans) { }
        public override bool Check()
        {
            Block target = MonoSingleton<World>.Instance.GetBlock(m_trans.position + Vector3.up);
            if (target == null) return false;
            return target.IsSolid(eDirection.down);
        }
    }
    public class Con_Blocked_Down : Con_TF
    {
        public Con_Blocked_Down(Transform trans) : base(trans) { }
        public override bool Check()
        {
            Block target = MonoSingleton<World>.Instance.GetBlock(m_trans.position + Vector3.down);
            if (target == null) return false;
            return target.IsSolid(eDirection.up);
        }
    }
}
