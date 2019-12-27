using UnityEngine;
using Assets.Scripts.Pattern;
using Assets.Scripts.BehaviorTree;
using Assets.Scripts.ActorSpace;

namespace Assets.Scripts.BehaviorTree
{
    class BevExecuter : MonoBehaviour
    {      
        private BevNodeBase m_Root;
        private ActorBevData m_bevData;


        private void Awake()
        {
            m_Root = ActorBevFactory.Instance.PLayerBev();
            
            Debug.Log(GetComponent<Actor>());

            m_bevData = new ActorBevData(GetComponent<Actor>());
            
        }
        private void Start()
        {          
           
        }
        private void Update()
        {
            m_Root.Evaluate(m_bevData);
        }
    }
}
