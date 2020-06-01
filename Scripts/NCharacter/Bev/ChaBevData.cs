using Assets.Scripts.NBehaviorTree;
using Assets.Scripts.NEvent;
using Assets.Scripts.NData;

using UnityEngine;

namespace Assets.Scripts.NCharacter
{
    public class ChaBevData : BevData
    {
        public ChaBevData(Character _Character, Communicator communicator)
        {
            Character = _Character;
            m_Communicator = communicator;

            Request_Translation = new Request<Vector3>();
            Request_Yaw = new Request<float>();
            Request_Jump = new Request<bool>();
        }

        public void NotifyOtherComponents(IEvent _event)
        {
            m_Communicator.PublishEvent(_event);
        }

        private Communicator m_Communicator;

        public bool isWalking;
        public bool isGrounded;
        public bool isInAir;
        //public bool JumpRequest;
        public Character Character;

        public Request<bool> Request_Jump;
        public Request<Vector3> Request_Translation;
        public Request<float> Request_Yaw;
    }

}
