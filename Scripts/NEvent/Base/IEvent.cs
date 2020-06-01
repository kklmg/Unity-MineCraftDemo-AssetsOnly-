using System;
using UnityEngine;


namespace Assets.Scripts.NEvent
{
    public interface IEvent
    {
        Guid Type { get; }
        byte Priority { get; }
    }

    public abstract class EventBase<T> : IEvent
    {
        [SerializeField]
        public static readonly Guid ID = Guid.NewGuid();

        [SerializeField]
        private byte m_Priority = 0;

        [SerializeField]
        private bool m_IsHandled = false;

        //property
        public Guid Type { get { return ID; } }
        public byte Priority { get { return m_Priority; } }

        protected void SetPriority(byte priority)
        {
            m_Priority = priority;
        }
    }
}
