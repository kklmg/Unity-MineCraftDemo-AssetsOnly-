using UnityEngine;
using Assets.Scripts.Pattern;
using Assets.Scripts.BehaviorTree;
using Assets.Scripts.CharacterSpace;

namespace Assets.Scripts.BehaviorTree
{
    class BevExecuter : MonoBehaviour
    {      
        private BevNodeBase m_Root;
        private ChaBevData m_bevData;

        private void Awake()
        {
            m_Root = ChaBevFactory.Instance.Base_ChaControl();            
            m_bevData = new ChaBevData(GetComponent<Character>());            
        }

        private void Update()
        {
            m_Root.Evaluate(m_bevData);
        }
    }
}
