using UnityEngine;
using Assets.Scripts.WorldComponent;
using Assets.Scripts.BehaviorTree;

namespace Assets.Scripts.Action
{
    public class goup : BevLeaf
    {
        [SerializeField]
        Transform m_Trans;

        public goup(Transform trans)
        {
            m_Trans = trans;
        }

        protected override eNodeState Tick()
        {
            m_Trans.Translate(Vector3.up);
            return eNodeState.Suceed;
        }
    }
}
