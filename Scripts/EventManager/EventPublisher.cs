﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.EventManager
{
    public interface IEventPublisher
    {
        void Publish(IEvent _event);
    }
    public class EventPublisher : IEventPublisher
    {
        private IEveneCenter m_refEventCenter;

        public EventPublisher(IEveneCenter EventMng)
        {
            m_refEventCenter = EventMng;
        }

        public void Publish(IEvent _event)
        {
            m_refEventCenter.GetEvent(_event);
        }

    }
}