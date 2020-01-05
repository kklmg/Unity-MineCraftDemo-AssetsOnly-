using UnityEngine;
using Assets.Scripts.Pattern;
using Assets.Scripts.BehaviorTree;
using Assets.Scripts.EventManager;
using Assets.Scripts.CharacterSpace;

namespace Assets.Scripts.BehaviorTree
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(ComponentCommunicator))]
    class BevExecuter : MonoBehaviour
    {      
        private BevNodeBase m_Root;
        private ChaBevData m_bevData;

        private void Awake()
        {
            m_Root = ChaBevFactory.Instance.ChaControl_Base();            
            m_bevData = new ChaBevData(GetComponent<Character>(),GetComponent<ComponentCommunicator>());            
        }

        private void Update()
        {
            m_Root.Evaluate(m_bevData);
        }
    }
}
