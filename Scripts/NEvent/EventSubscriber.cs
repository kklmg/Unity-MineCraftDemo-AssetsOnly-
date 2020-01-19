using System;

namespace Assets.Scripts.NEvent
{
    public interface IEventSubscriber
    {
        void Subscribe(Guid EventID, Del_HandleEvent handler, bool HighPriority = false);
    }

    class EventSubscriber : IEventSubscriber
    {
        private IEveneCenter m_refEventCenter;

        //Constructor
        public EventSubscriber(IEveneCenter EventCenter)
        {
            m_refEventCenter = EventCenter;
        }

        //subscribe
        public void Subscribe(Guid EventID, Del_HandleEvent handler, bool HighPriority = false)
        {
            m_refEventCenter.SubScribe(EventID, handler, HighPriority);
        }
    }
}
