using Assets.Scripts.NCharacter;
using UnityEngine;

namespace Assets.Scripts.NEvent
{
    //Event Character Movement
    public class E_Cha_MoveRequest_XZ : EventBase<E_Cha_MoveRequest_XZ>
    {
        public Vector3 Translation { get; set; }

        public E_Cha_MoveRequest_XZ(Vector3 value)
        {
            Translation = value;
        }
    }

    public class E_Cha_MoveRequest_Y : EventBase<E_Cha_MoveRequest_Y>
    {
        public float Speed { get; set; }

        public E_Cha_MoveRequest_Y(float _Speed)
        {
            SetPriority(5);
            Speed = _Speed;
        }
    }

    public class E_Cha_YawRequest : EventBase<E_Cha_YawRequest>
    {
        public float Value {set; get; }
        public E_Cha_YawRequest(float _value)
        {
            Value = _value;
        }
    }

    public class E_Cha_RotateRequest : EventBase<E_Cha_RotateRequest>
    {
        public Vector3 Rotation { get; }
        public E_Cha_RotateRequest(Vector3 value)
        {
            Rotation = value;
        }
    }


    public class E_Cha_Spawned : EventBase<E_Cha_Spawned>
    {
    }
    public class E_Cha_TouchGround : EventBase<E_Cha_TouchGround>
    {
    }
    public class E_Cha_TouchUpsideBlock : EventBase<E_Cha_TouchUpsideBlock>
    {
    }



    public class E_Cha_StartJump : EventBase<E_Cha_StartJump>
    {
        private float m_Force;
        public float Force { get { return m_Force; } }

        public E_Cha_StartJump(float force)
        {
            m_Force = force;
        }
    }


    public class E_Cha_Moved_XZ : EventBase<E_Cha_Moved_XZ>
    {
    }
}
