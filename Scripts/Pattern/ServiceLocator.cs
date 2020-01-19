using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Pattern
{
    public static class Locator<T>
    {
        private static T m_Service;
        public static T GetService()
        {
            return m_Service;
        }
        public static void SubscribeService(MyObserver<T> observer)
        {
            m_Observers.Add(observer);
            observer.Update(m_Service);
        }
        public static void ProvideService(T _service)
        {
            m_Service = _service;

            foreach(var observer in m_Observers)
            {
                observer.Update(m_Service);
            }
        }

        static private List<MyObserver<T>> m_Observers;  //Observers
    }
}