using System;
using Assets.Scripts.Pattern;
using Assets.Scripts.WorldComponent;
using Assets.Scripts.EventManager;
using UnityEngine;

namespace Assets.Scripts.CharacterSpace
{
    [RequireComponent(typeof(ComponentCommunicator))]
    class ChaCollider : MonoBehaviour
    {        
        void Start()
        {
            GetComponent<ComponentCommunicator>().SubsribeEvent(E_Cha_TryMove.ID, Handle);
        }
        private bool Handle(IEvent _event)
        {
            var Data = (_event as E_Cha_TryMove);
            World world = Locator<World>.GetService();
            ChaBevData ThisData = Data.Cha_Data;
     
            Block adj;

            //right
            if (ThisData.Movement.x > 0)
            {
                adj = world.GetBlock(ThisData.Character.transform.position + Vector3.right);
                if (adj != null && adj.IsSolid(eDirection.left))
                {
                    ThisData.Movement.x = Mathf.Min(ThisData.Movement.x, 1 - (transform.position.x % 1));
                   // ThisData.Movement.x = 0;
                }
            }
            //left
            else
            {
                adj = world.GetBlock(ThisData.Character.transform.position + Vector3.left);
                if (adj != null && adj.IsSolid(eDirection.left))
                {
                    ThisData.Movement.x = Mathf.Min(ThisData.Movement.x, 1 - (transform.position.x % 1));
                    //ThisData.Movement.x = 0;
                }
            }

            //forward
            if (ThisData.Movement.z > 0)
            {
                adj = world.GetBlock(ThisData.Character.transform.position + Vector3.forward);
                if (adj != null && adj.IsSolid(eDirection.backward))
                {
                    ThisData.Movement.z = 0;
                    //movement.z = Mathf.Min(movement.z, 1 - (trans.position.z % 1));
                    Debug.Log(ThisData.Movement);
                }
            }
            //backward
            else
            {
                adj = world.GetBlock(ThisData.Character.transform.position + Vector3.back);
                if (adj != null && adj.IsSolid(eDirection.forward))
                {
                    ThisData.Movement.z = 0;
                    //movement.z = Mathf.Min(movement.z, 1 - (trans.position.z % 1));
                    Debug.Log(ThisData.Movement);
                }
            }
       

            ////up
            //if (movement.y > 0)
            //{
            //    adj = world.GetBlock(trans.position + Vector3.up);
            //    if (adj != null && adj.IsSolid(eDirection.down))
            //    {
            //        movement.y = Mathf.Min(movement.y, 1 - (trans.position.y % 1));
            //        Debug.Log(movement);
            //    }
            //}
            ////down
            //else
            //{
            //    adj = world.GetBlock(trans.position + Vector3.down);
            //    if (adj != null && adj.IsSolid(eDirection.up))
            //    {
            //        movement.y = Mathf.Min(movement.y, 1 - (trans.position.y % 1));
            //        Debug.Log(movement);
            //    }
            //}
            return true;
        }
    }

}

 


