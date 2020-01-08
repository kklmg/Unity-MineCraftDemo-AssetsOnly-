using Assets.Scripts.BehaviorTree;
using Assets.Scripts.EventManager;

using UnityEngine;

namespace Assets.Scripts.CharacterSpace
{
    public class ChaMove
    {
        public float Trans_x;
        public float Trans_y;
        public float Trans_z;
        public float Rotation_X;
        public float Rotation_Y;
        public float Rotation_Z;
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
