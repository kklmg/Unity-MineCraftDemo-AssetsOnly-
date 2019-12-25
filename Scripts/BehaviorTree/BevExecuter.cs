using UnityEngine;
using Assets.Scripts.Pattern;

namespace Assets.Scripts.BehaviorTree
{
    class BevExecuter : MonoBehaviour
    {      
        private BevNodeBase m_Root;
        private BlackBoard m_BlackBoard;

        private void Start()
        {
            BevFactory fact = new BevFactory();
            m_Root = fact.MakePlayerInputBehavior(transform);
        }
        private void Update()
        {
            m_Root.Evaluate();
        }


    }
}
