using UnityEngine;

using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NEvent;

namespace Assets.Scripts.NGameSystem
{
    class EventMng : MonoBehaviour
    {
        private EventCenter m_EventCenter;

        public void InitEventService()
        {
            m_EventCenter = new EventCenter();

            EventPublisher publisher = new EventPublisher(m_EventCenter);
            EventSubscriber Subscriber = new EventSubscriber(m_EventCenter);

            Locator<IEventPublisher>.ProvideService(publisher);
            Locator<IEventSubscriber>.ProvideService(Subscriber);
        }

        private void LateUpdate()
        {
            if (m_EventCenter != null)
                m_EventCenter.HandleAll();
        }
    }
}
