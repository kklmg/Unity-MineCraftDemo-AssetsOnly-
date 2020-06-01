using System;

namespace Assets.Scripts.NEvent
{
    class EventHelper : IEventHelper
    {
        private IEventCenter m_EventCenter;

        //Constructor
        public EventHelper(IEventCenter EventCenter)
        {
            m_EventCenter = EventCenter;
        }

        public void Subscribe(Guid EventID, Del_HandleEvent handler, byte HandlePriority)
        {
            m_EventCenter.SubScribe(EventID, handler,HandlePriority);
        }

        public void UnSubscribe(Guid EventID, Del_HandleEvent handler)
        {
            m_EventCenter.UnSubScribe(EventID, handler);
        }

        public void Publish(IEvent _event)
        {
            m_EventCenter.AddEvent(_event);
        }

        public void PublishAndHandle(IEvent _event)
        {
            m_EventCenter.Handle(_event);
        }
    }
}
