using Assets.Scripts.BehaviorTree;
using UnityEngine;

namespace Assets.Scripts.ActorSpace
{
    public class ActorBevData : BevData
    {
        public ActorBevData(Actor _actor)
        {
            actor = _actor;
        }

        public const string KEY_VELOCITY = "velocity";

        public bool isWalking;
        public bool isGrounded;
        public bool isJumping;
        public Actor actor;
    }
}
