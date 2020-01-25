using UnityEngine;
using Assets.Scripts.NWorld;
using Assets.Scripts.NBehaviorTree;

namespace Assets.Scripts.NAction
{
    public class goup : BevLeaf
    {
        [SerializeField]
        Transform m_Trans;

        public goup(Transform trans)
        {
            m_Trans = trans;
        }

        protected override eRunningState Tick(BevData workData)
        {
            m_Trans.Translate(Vector3.up);
            return eRunningState.Suceed;
        }
    }
}
