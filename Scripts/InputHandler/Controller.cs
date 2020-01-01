using UnityEngine;
using Assets.Scripts.BehaviorTree;

namespace Assets.Scripts.InputHandler
{
    public interface IController
    {
        float Horizontal();
        float Vertical();
        bool Fire();
        bool Jump();
        bool Sprint();
        float Rotate_X();
        float Rotate_Y();
        float Rotate_Z();
    }

    public class Controller_PC : IController
    {
        public float Horizontal()
        {
            return Input.GetAxis(KEY_INPUT.HORIZONTAL);
        }
        public float Vertical()
        {
            return Input.GetAxis(KEY_INPUT.VERTICAL);
        }
        public bool Fire()
        {
            return Input.GetButtonDown(KEY_INPUT.ATTACK);
        }
        public bool Jump()
        {
            return Input.GetButtonDown(KEY_INPUT.JUMP);
        }
        public bool Sprint()
        {
            return Input.GetButtonDown(KEY_INPUT.RUN);
        }

        public float Rotate_X()
        {
            return Input.GetAxis(KEY_INPUT.MOUSE_VERTICAL);
        }
        public float Rotate_Y()
        {
            return Input.GetAxis(KEY_INPUT.MOUSE_HORIZONTAL);
        }
        public float Rotate_Z()
        {
            return 0;
            //return Input.GetAxis(KEY_INPUT.ROTATION_Z);
        }
    }
}
