using System;
using Assets.Scripts.Pattern;
using Assets.Scripts.WorldComponent;
using Assets.Scripts.EventManager;
using UnityEngine;

namespace Assets.Scripts.CharacterSpace
{
    [RequireComponent(typeof(Communicator))]
    class ChaCollider : MonoBehaviour
    {        
        void Start()
        {
            GetComponent<Communicator>().SubsribeEvent(E_Cha_TryMove.ID, Handle);
        }
        private bool Handle(IEvent _event)
        {
            var Data = (_event as E_Cha_TryMove);
            World world = Locator<World>.GetService();
            ChaMove ThisData = Data.Move_Data;
     
            Block adj;

            //right
            if (ThisData.Trans_x > 0)
            {
                adj = world.GetBlock(transform.position + Vector3.right);
                if (adj != null && adj.IsSolid(eDirection.left))
                {
                    ThisData.Trans_x = Mathf.Min(ThisData.Trans_x, 1 - (transform.position.x % 1));
                   // ThisData.Movement.x = 0;
                }
            }
            //left
            else
            {
                adj = world.GetBlock(transform.position + Vector3.left);
                if (adj != null && adj.IsSolid(eDirection.left))
                {
                    ThisData.Trans_x = Mathf.Min(ThisData.Trans_x, 1 - (transform.position.x % 1));
                    //ThisData.Movement.x = 0;
                }
            }

            //forward
            if (ThisData.Trans_z > 0)
            {
                adj = world.GetBlock(transform.position + Vector3.forward);
                if (adj != null && adj.IsSolid(eDirection.backward))
                {
                    ThisData.Trans_z = 0;
                    //movement.z = Mathf.Min(movement.z, 1 - (trans.position.z % 1));
                }
            }
            //backward
            else
            {
                adj = world.GetBlock(transform.position + Vector3.back);
                if (adj != null && adj.IsSolid(eDirection.forward))
                {
                    ThisData.Trans_z = 0;
                    //movement.z = Mathf.Min(movement.z, 1 - (trans.position.z % 1));
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

 


