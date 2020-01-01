using Assets.Scripts.EventManager;
using UnityEngine;
using Assets.Scripts.Pattern;

namespace Assets.Scripts.CharacterSpace
{
    public class Event_Character_Transform : EventBase<Event_Character_Transform>
    {
        public Event_Character_Transform(Transform trans)
        {
            
        }

    }




    public class Event_Character_TryMove : EventBase<Event_Character_TryMove>
    {
        public Event_Character_TryMove(Vector3 velocity)
        {
            m_Velocity = velocity;
        }

        public Vector3 m_Velocity;
    }

    public class Event_Character_Movement : EventBase<Event_Character_Movement>
    {
    }




    // ServiceLocator<IEventPublisher>.GetService().Publish(this);



}
