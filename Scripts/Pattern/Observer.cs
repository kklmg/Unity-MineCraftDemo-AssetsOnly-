using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Pattern
{
    public class MyObserver<TData> where TData : new()
    {
        //Field
        //---------------------------------------------------------------
        //UnSubscriber
        private Unsubscriber<TData> m_UnSubscriber;
        //Data
        private TData m_Data;

        //Property
        //---------------------------------------------------------------
        public bool IsDirty{ set; get; }      
        public TData Data { get { return m_Data; } }

        //public Function
        //---------------------------------------------------------------
        public void Update(TData _data)
        {
            m_Data = _data;
            IsDirty = true;
        }

        //Subscribe Subject
        public void Subscribe(MySubject<TData> _subject)
        {
            Unsubscribe();
            _subject.Subscribe(this);
        }

        //Unsubscribe Subject
        public void Unsubscribe()
        {
            m_UnSubscriber.Unsubscribe();
        }
    }


    //class Subject Base

    public class MySubject<TData> where TData : new()
    {
        //Field 
        [SerializeField]
        private TData m_Data = new TData();

        //Observers
        private List<MyObserver<TData>> m_listObservers;

        //update Subject and notify to all Observers
        public void Update(TData _data)
        {
            m_Data = _data;
            __notify();
        }

        //notify to all observers
        private void __notify()
        {
            foreach (var observer in m_listObservers)
            {
                observer.Update(m_Data);              
            }
        }

        //Add Observer
        public Unsubscriber<TData> Subscribe(MyObserver<TData> _Observer)
        {
            m_listObservers.Add(_Observer);
            return new Unsubscriber<TData>(m_listObservers, _Observer);
        }
    }


    //Class Unsubscriber
    public class Unsubscriber<TData> where TData : new()
    {
        private List<MyObserver<TData>> m_listObeservers;
        private MyObserver<TData> m_Observer;

        public Unsubscriber(List<MyObserver<TData>> observers, MyObserver<TData> observer)
        {
            this.m_listObeservers = observers;
            this.m_Observer = observer;
        }

        public void Unsubscribe()
        {
            if (m_Observer != null && m_listObeservers.Contains(m_Observer))
                m_listObeservers.Remove(m_Observer);
        }
    }

}
