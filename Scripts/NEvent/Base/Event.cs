using System;
using UnityEngine;


namespace Assets.Scripts.NEvent
{
    public interface IEvent
    {
        Guid Type { get; }
    }

    public abstract class EventBase<T> : IEvent
    {
        [SerializeField]
        public static readonly Guid ID = Guid.NewGuid();
        //property
        public Guid Type { get { return ID; } }
    }
}
