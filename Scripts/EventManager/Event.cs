using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets.Scripts.EventManager
{
    [Serializable]
    public class Unique<T>
    {
        public static readonly Guid ID = Guid.NewGuid();
    } 
    public interface IEvent
    {
        Guid Type { get; }
    }
    public abstract class EventBase<T> : IEvent
    {
        [SerializeField]
        private Unique<T> m_guid;
        //property
        public Guid Type { get { return Unique<T>.ID; } }
    }
}
