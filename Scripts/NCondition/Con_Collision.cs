using UnityEngine;
using Assets.Scripts.NWorld;
using Assets.Scripts.NPattern;

namespace Assets.Scripts.NCondition
{
    public abstract class Con_Transform : ConditionBase
    {
        protected Transform m_trans;
        public Con_Transform(Transform trans)
        {
            m_trans = trans;
        }       
    }  
    //public class Con_Blocked_left : Con_Transform
    //{
    //    public Con_Blocked_left(Transform trans) : base(trans) { }
    //    public override bool Check()
    //    {
    //        Block target = MonoSingleton<World>.Instance.GetBlock(m_trans.position + Vector3.left);
    //        if (target == null) return false;
    //        return target.IsSolid(Direction.RIGHT);
    //    }
    //}
    //public class Con_Blocked_Right : Con_Transform
    //{
    //    public Con_Blocked_Right(Transform trans) : base(trans) { }
    //    public override bool Check()
    //    {
    //        Block target = MonoSingleton<World>.Instance.GetBlock(m_trans.position + Vector3.right);
    //        if (target == null) return false;
    //        return target.IsSolid(Direction.LEFT);
    //    }
    //}
    //public class Con_Blocked_Front : Con_Transform
    //{
    //    public Con_Blocked_Front(Transform trans) : base(trans) { }
    //    public override bool Check()
    //    {
    //        Block target = MonoSingleton<World>.Instance.GetBlock(m_trans.position + Vector3.forward);
    //        if (target == null) return false;
    //        return target.IsSolid(Direction.BACKWARD);
    //    }
    //}
    //public class Con_Blocked_Back : Con_Transform
    //{
    //    public Con_Blocked_Back(Transform trans) : base(trans) { }
    //    public override bool Check()
    //    {
    //        Block target = MonoSingleton<World>.Instance.GetBlock(m_trans.position + Vector3.back);
    //        if (target == null) return false;
    //        return target.IsSolid(Direction.FORWARD);
    //    }
    //}
    //public class Con_Blocked_Up : Con_Transform
    //{
    //    public Con_Blocked_Up(Transform trans) : base(trans) { }
    //    public override bool Check()
    //    {
    //        Block target = MonoSingleton<World>.Instance.GetBlock(m_trans.position + Vector3.up);
    //        if (target == null) return false;
    //        return target.IsSolid(Direction.DOWN);
    //    }
    //}
    //public class Con_Blocked_Down : Con_Transform
    //{
    //    public Con_Blocked_Down(Transform trans) : base(trans) { }
    //    public override bool Check()
    //    {
    //        Block target = MonoSingleton<World>.Instance.GetBlock(m_trans.position + Vector3.down);
    //        if (target == null) return false;
    //        return target.IsSolid(Direction.UP);
    //    }
    //}
}
