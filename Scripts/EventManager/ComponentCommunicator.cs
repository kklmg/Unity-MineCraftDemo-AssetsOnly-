using System;
using UnityEngine;

namespace Assets.Scripts.EventManager
{
    public class ComponentCommunicator : MonoBehaviour
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
        private void Update()
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
    }
}
