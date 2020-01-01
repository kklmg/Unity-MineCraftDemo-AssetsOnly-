using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public interface IEventData
    {
        Guid GetEventType();
        float GetTimeStamp();

        IEventData Copy();
        string GetName();
    };


  
    [System.Serializable]
    public class Event : MonoBehaviour
    {
        public UnityAction abc;
        public UnityEvent asdfasdf;
        public UnityEvent<int> asdf;
        public UnityEvent<int,int,int> addsdf;
        public Animation ls;


        private void Awake()
        {
        }
        
        //chec

        public void gaogogo()
        {
            //abc = new UnityAction();
           
            

        }

        public void myfunction() {

            Debug.Log("aaa");

        }


       
    }

   
    //abstract class BaseEventData : IEventData
    //{
    //    //Field
    //    float m_timeStamp;

    //    public BaseEventData(float timeStamp = 0.0f)
    //    {
    //        m_timeStamp = timeStamp;
    //    }



    //    public Guid abstract override GetEventType();
    //    public float GetTimeStamp();

    //    public IEventData Copy();
    //    public string GetName();
    //}

}


