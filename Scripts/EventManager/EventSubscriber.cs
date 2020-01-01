using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.EventManager
{
    public interface IEventSubscriber
    {
        void Subscribe(Guid EventID, IEventHandler handler, bool HighPriority = false);
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
        public void Subscribe(Guid EventID, IEventHandler handler, bool HighPriority = false)
        {
            m_refEventCenter.SubScribe(EventID, handler, HighPriority);
        }

    }
}
