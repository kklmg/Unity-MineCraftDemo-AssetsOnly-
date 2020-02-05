using UnityEngine;
using Assets.Scripts.NGlobal.Singleton;
using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NEvent;

namespace Assets.Scripts.NGameMng
{
    class EventMng : MonoSingleton<EventMng>
    {
        private EventCenter m_EventCenter;

        private void Awake()
        {
            m_EventCenter = new EventCenter();

            EventPublisher publisher = new EventPublisher(m_EventCenter);
            EventSubscriber Subscriber = new EventSubscriber(m_EventCenter);

            Locator<IEventPublisher>.ProvideService(publisher);
            Locator<IEventSubscriber>.ProvideService(Subscriber);
        }
        //private void Update()
        //{
        //    m_EventCenter.HandleAll();
        //}

        private void LateUpdate()
        {
            m_EventCenter.HandleAll();
        }
    }
}
