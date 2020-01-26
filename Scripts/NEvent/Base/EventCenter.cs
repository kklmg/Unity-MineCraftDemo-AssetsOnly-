using System;
using System.Collections.Generic;

namespace Assets.Scripts.NEvent
{
    public static class SubsribeOption
    {
        public const byte NO_OPTION =  0x00;
        public const byte LOW_PRIORITY =  0x01;
        public const byte HIGH_PRIORITY = 0x0f;
        //public const byte DECORATE = 0x10;
    }


    public interface IEveneCenter
    {
        void GetEvent(IEvent _event);
        void SubScribe(Guid id, Del_HandleEvent handler, byte option = SubsribeOption.NO_OPTION);
        void SubScribe_Decorate(Guid id, Del_DecorateEvent decorator, byte option = SubsribeOption.NO_OPTION);
        void Handle(IEvent _event);
        void HandleAll();
    }

    class EventCenter : IEveneCenter
    {
        //Field 
        //-----------------------------------------------------------------------------
        Dictionary<Guid, LinkedList<Del_HandleEvent>> m_EventHandlers; //Event listeners
        Dictionary<Guid, LinkedList<Del_DecorateEvent>> m_EventDecorators; //Event Decorators

        Queue<IEvent> m_EventQueue; //Event Storage

        //Cache
        IEvent m_Cache_Event;
        LinkedList<Del_HandleEvent> m_Cache_Listeners;
        LinkedList<Del_DecorateEvent> m_Cache_Decorators;

        //Constructor
        //-----------------------------------------------------------------------------
        public EventCenter()
        {
            m_EventHandlers = new Dictionary<Guid, LinkedList<Del_HandleEvent>>();
            m_EventDecorators = new Dictionary<Guid, LinkedList<Del_DecorateEvent>>();
            m_EventQueue = new Queue<IEvent>();
        }

        //public Functions
        //-----------------------------------------------------------------------------
        public void GetEvent(IEvent _event)
        {
            m_EventQueue.Enqueue(_event);
        }
        public void SubScribe(Guid EventType, Del_HandleEvent handler, byte option = SubsribeOption.NO_OPTION)
        {
            //Case: Exist listener list
            if (m_EventHandlers.TryGetValue(EventType, out m_Cache_Listeners))
            {
                if (option == SubsribeOption.HIGH_PRIORITY)
                    m_Cache_Listeners.AddFirst(handler);

                else if (option ==SubsribeOption.LOW_PRIORITY)
                    m_Cache_Listeners.AddLast(handler);

                else
                    m_Cache_Listeners.AddLast(handler);
            }
            //Case: No listener List
            else
            {
                m_Cache_Listeners = new LinkedList<Del_HandleEvent>();
                m_Cache_Listeners.AddLast(handler);
                m_EventHandlers.Add(EventType, m_Cache_Listeners);
            }
        }
        public void SubScribe_Decorate(Guid EventType, Del_DecorateEvent decorator, byte option = SubsribeOption.NO_OPTION)
        {
            //Case: Exist listener list
            if (m_EventDecorators.TryGetValue(EventType, out m_Cache_Decorators))
            {
                if (option == SubsribeOption.HIGH_PRIORITY)
                    m_Cache_Decorators.AddFirst(decorator);

                else if (option == SubsribeOption.LOW_PRIORITY)
                    m_Cache_Decorators.AddLast(decorator);

                else
                    m_Cache_Decorators.AddLast(decorator);
            }
            //Case: No listener List
            else
            {
                m_Cache_Decorators = new LinkedList<Del_DecorateEvent>();
                m_Cache_Decorators.AddLast(decorator);
                m_EventDecorators.Add(EventType, m_Cache_Decorators);
            }
        }

        public void Handle(IEvent _event)
        {
            //find mapped decorators
            if (m_EventDecorators.TryGetValue(_event.Type, out m_Cache_Decorators))
            {
                foreach (var Decorator in m_Cache_Decorators)
                {
                    //Decorate event
                    _event = Decorator(_event);
                }
            }

            //find mapped listerners 
            if (m_EventHandlers.TryGetValue(_event.Type, out m_Cache_Listeners))
            { 
                foreach (var Handler in m_Cache_Listeners)
                {
                    //Handle event
                    Handler(_event);
                }
            }
        }

        public void HandleAll()
        {
            while (m_EventQueue.Count != 0)
            {
                //event wiil be handled
                m_Cache_Event = m_EventQueue.Dequeue();
                Handle(m_Cache_Event);
            }
        }
    }
}
