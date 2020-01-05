using Assets.Scripts.BehaviorTree;
using Assets.Scripts.EventManager;

using UnityEngine;

namespace Assets.Scripts.CharacterSpace
{
    public class ChaBevData : BevData
    {
        public ChaBevData(Character _Character, ComponentCommunicator communicator)
        {
            Character = _Character;
            Communicator = communicator;
        }

        public float Rotation_X;
        public float Rotation_Y;
        public float Rotation_Z;

        public Vector3 Movement;
        public bool isWalking;
        public bool isGrounded;
        public bool isJumping;
        public Character Character;

        public ComponentCommunicator Communicator;
    }



}
