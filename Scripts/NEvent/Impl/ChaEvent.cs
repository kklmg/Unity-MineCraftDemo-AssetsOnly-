using Assets.Scripts.NCharacter;
using UnityEngine;

namespace Assets.Scripts.NEvent
{
    //The Character want to move
    public class E_Cha_MoveRequest : EventBase<E_Cha_MoveRequest>
    {
        public Vector3 Translation { get; set; }
        public E_Cha_MoveRequest(Vector3 value)
        {
            Translation = value;
        }
    }

    //The Character want to rotate
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

    public class E_Cha_Jump : EventBase<E_Cha_Jump>
    {
        private float m_Force;
        public float Force { get { return m_Force; } }

        public E_Cha_Jump(float force)
        {
            m_Force = force;
        }
    }

    public class E_Cha_Moved : EventBase<E_Cha_Moved>
    {
        //character reference
        public Character Cha { get; set; }

        //Contructor;
        public E_Cha_Moved() { }
        public E_Cha_Moved(Character cha)
        {
            Cha = cha;
        }
    }

    // Locator<IEventPublisher>.GetService().Publish(this);
}
