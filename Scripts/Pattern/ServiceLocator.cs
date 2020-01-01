using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Pattern
{
    public class ServiceLocator<T>
    {
        private static T m_Service;
        public static T GetService()
        {
            return m_Service;
        }
        public static void ProvideService(T _service)
        {
            m_Service = _service;
        }
    }
}