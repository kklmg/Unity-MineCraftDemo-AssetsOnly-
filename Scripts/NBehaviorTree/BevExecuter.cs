using UnityEngine;
using Assets.Scripts.NPattern;
using Assets.Scripts.NBehaviorTree;
using Assets.Scripts.NEvent;
using Assets.Scripts.NCharacter;

namespace Assets.Scripts.NBehaviorTree
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(Communicator))]
    class BevExecuter : MonoBehaviour
    {      
        private BevNodeBase m_Root;
        private ChaBevData m_bevData;

        private void Awake()
        {
            m_Root = ChaBevFactory.Instance.ChaControl_Base();            
            m_bevData = new ChaBevData(GetComponent<Character>(),GetComponent<Communicator>());            
        }

        private void Update()
        {
            m_Root.Evaluate(m_bevData);
        }
    }
}
