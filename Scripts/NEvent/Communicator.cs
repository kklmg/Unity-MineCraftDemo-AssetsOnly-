using System;
using UnityEngine;

namespace Assets.Scripts.NEvent
{
    public class Communicator : MonoBehaviour
    {
        private EventCenter m_EventCenter;
        private EventHelper m_EventHelper;

        private void Awake()
        {
            m_EventCenter = new EventCenter(10);
            m_EventHelper = new EventHelper(m_EventCenter);
        }

        private void LateUpdate()
        {
            m_EventCenter.HandleAll();
        }

        public void PublishEvent(IEvent _event)
        {
            m_EventHelper.PublishAndHandle(_event);
        }

        public void SubsribeEvent(Guid EventID, Del_HandleEvent handler,byte priority)
        {
            m_EventHelper.Subscribe(EventID, handler,priority);
        }

        public void UnSubscribe(Guid EventID, Del_HandleEvent handler)
        {
            m_EventHelper.UnSubscribe(EventID, handler);
        }
    }
}
