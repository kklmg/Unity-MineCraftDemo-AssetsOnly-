using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets.Scripts.EventManager
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
