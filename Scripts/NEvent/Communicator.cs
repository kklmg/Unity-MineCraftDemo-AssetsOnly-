using System;
using UnityEngine;

namespace Assets.Scripts.NEvent
{
    public class Communicator : MonoBehaviour
    {
        private EventCenter m_EventCenter;
        private EventPublisher m_Publisher;
        private EventSubscriber m_Subscriber;

        private void Awake()
        {
            m_EventCenter = new EventCenter();
            m_Publisher = new EventPublisher(m_EventCenter);
            m_Subscriber = new EventSubscriber(m_EventCenter);
        }

        private void LateUpdate()
        {
            m_EventCenter.HandleAll();
        }

        public void PublishEvent(IEvent _event)
        {
            m_Publisher.PublishAndHandle(_event);
        }

        public void SubsribeEvent(Guid EventID, Del_HandleEvent handler)
        {
            m_Subscriber.Subscribe(EventID, handler);
        }
        public void SubsribeEvent_Decorate(Guid EventID, Del_DecorateEvent decorator)
        {
            m_Subscriber.Subscribe_Deccorate(EventID, decorator);
        }
    }
}
