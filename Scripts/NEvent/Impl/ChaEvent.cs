using Assets.Scripts.NCharacter;

namespace Assets.Scripts.NEvent.Impl
{
    public class E_Cha_TryMove : EventBase<E_Cha_TryMove>
    {
        public ChaMove Move_Data { get; }
        public E_Cha_TryMove(ChaMove data)
        {
            Move_Data = data;
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
