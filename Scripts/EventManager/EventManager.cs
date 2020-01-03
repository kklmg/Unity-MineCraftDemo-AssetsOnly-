using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Pattern;

namespace Assets.Scripts.EventManager
{
    class EventManager : MonoBehaviour
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
        private void Update()
        {
            m_EventCenter.HandleAll();
        }
    }
}
