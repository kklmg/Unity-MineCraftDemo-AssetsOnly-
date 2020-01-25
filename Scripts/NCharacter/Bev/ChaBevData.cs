using Assets.Scripts.NBehaviorTree;
using Assets.Scripts.NEvent;

using UnityEngine;

namespace Assets.Scripts.NCharacter
{
    public class ChaMove
    {
        public Vector3 Translation;
        public Vector3 Rotation;
    }
    public class ChaBevData : BevData
    {
        public ChaBevData(Character _Character, Communicator communicator)
        {
            Character = _Character;
            Communicator = communicator;
            Move = new ChaMove();
        }

        public bool isWalking;
        public bool isGrounded;
        public bool isJumping;
        public Character Character;
        public ChaMove Move;
        public Communicator Communicator;
    }



}
