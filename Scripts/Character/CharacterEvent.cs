using Assets.Scripts.EventManager;
using UnityEngine;
using Assets.Scripts.Pattern;

namespace Assets.Scripts.CharacterSpace
{
    public class E_Cha_TryMove : EventBase<E_Cha_TryMove>
    {
        public Transform Trans { set; get; }
        public  Vector3 Movement { set; get; }
        public E_Cha_TryMove() { }
        public E_Cha_TryMove(ref Transform _trans,ref Vector3 _move)
        {
            Trans = _trans;
            Movement = _move;
        }
    }


    public class E_Cha_Moved : EventBase<E_Cha_Moved>
    {
        //character reference
        public Character Cha { get; set; }

        //Contructor;
        public E_Cha_Moved() { }
        public E_Cha_Moved(Character cha)
        {
            Cha = cha;
        }
    }






    // Locator<IEventPublisher>.GetService().Publish(this);



}
