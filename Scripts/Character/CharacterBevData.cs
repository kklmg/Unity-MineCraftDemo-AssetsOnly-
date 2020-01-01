using Assets.Scripts.BehaviorTree;
using UnityEngine;

namespace Assets.Scripts.CharacterSpace
{
    public class ChaBevData : BevData
    {
        public ChaBevData(Character _Character)
        {
            Character = _Character;
        }

        public Vector3 Movement;
        public float Rotation_X;
        public float Rotation_Y;
        public float Rotation_Z;

        public bool isWalking;
        public bool isGrounded;
        public bool isJumping;
        public Character Character;
    }



}
