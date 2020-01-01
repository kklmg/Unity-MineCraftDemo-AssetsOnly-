using System;
using System.Collections.Generic;
using Assets.Scripts.Pattern;

namespace Assets.Scripts.EventManager
{
    public interface IEveneCenter
    {
        void GetEvent(IEvent _event);
        void SubScribe(Guid id, IEventHandler handler,bool HighPriority); 
        void Handle(IEvent _event);
        void HandleAll();
    }

    class EventCenter : IEveneCenter
    {
        //Field 
        //-----------------------------------------------------------------------------
        Dictionary<Guid, LinkedList<IEventHandler>> m_ListenerMap; //Event listeners
        Queue<IEvent> m_EventQueue; 

        //Cache
        IEvent Cache_Event;
        LinkedList<IEventHandler> Cache_Listeners;

        //Constructor
        //-----------------------------------------------------------------------------
        public EventCenter()
        {
            m_ListenerMap = new Dictionary<Guid, LinkedList<IEventHandler>>();
            m_EventQueue = new Queue<IEvent>();
        }

        //public Functions
        //-----------------------------------------------------------------------------
        public void GetEvent(IEvent _event)
        {
            m_EventQueue.Enqueue(_event);
        }
        public void SubScribe(Guid EventType, IEventHandler handler,bool HighPriority = false)
        {
            //Case: Exist listener list
            if (m_ListenerMap.TryGetValue(EventType, out Cache_Listeners))
            {
                if (HighPriority)
                    Cache_Listeners.AddFirst(handler);
                else
                    Cache_Listeners.AddLast(handler);
            }
            //Case: No listener List
            else
            {
                Cache_Listeners = new LinkedList<IEventHandler>();
                Cache_Listeners.AddLast(handler);
                m_ListenerMap.Add(EventType, Cache_Listeners);
            }


        }
        public void Handle(IEvent _event)
        {
            //find mapped listerners 
            if (m_ListenerMap.TryGetValue(Cache_Event.Type, out Cache_Listeners))
            {
                foreach (var listener in Cache_Listeners)
                {
                    //Handle event
                    listener.Handle(Cache_Event);
                }
            }

        }
        public void HandleAll()
        {
            while (m_EventQueue.Count != 0)
            {
                //event wiil be handled
                Cache_Event = m_EventQueue.Dequeue();
            }
        }
    }

}
