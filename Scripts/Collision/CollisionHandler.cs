using Assets.Scripts.EventManager;
using Assets.Scripts.CharacterSpace;
using Assets.Scripts.Pattern;
using Assets.Scripts.WorldComponent;

using UnityEngine;

namespace Assets.Scripts.Collision
{
    class CollisionHandler : MonoBehaviour
    {
        void Start()
        {
            //Locator<IEventSubscriber>.GetService().
            //    Subscribe(E_Cha_TryMove.ID, HandleCollidion);
        }
        public bool HandleCollidion(IEvent _event)
        {
            return true;
            //var Data = (_event as E_Cha_TryMove);
            
            //Transform trans = Data.Trans;
            //Vector3 movement = Data.Movement;
            //World world = Locator<World>.GetService();

            //Debug.Log(world);
            //Block adj;

            ////right
            //if (movement.x > 0)
            //{
            //    adj = world.GetBlock(trans.position + Vector3.right);
            //    if (adj != null && adj.IsSolid(eDirection.left))
            //    {
            //        movement.x = Mathf.Min(movement.x, 1 - (trans.position.x % 1));
            //    }
            //}
            ////left
            //else
            //{
            //    adj = world.GetBlock(trans.position + Vector3.left);
            //    if (adj != null && adj.IsSolid(eDirection.left))
            //    {
            //        movement.x = Mathf.Min(movement.x, 1 - (trans.position.x % 1));
            //    }
            //}

            ////forward
            //if (movement.z > 0)
            //{
            //    adj = world.GetBlock(trans.position + Vector3.forward);
            //    if (adj != null && adj.IsSolid(eDirection.backward))
            //    {
            //        movement.z = Mathf.Min(movement.z, 1 - (trans.position.z % 1));
            //    }
            //}
            ////backward
            //else
            //{
            //    adj = world.GetBlock(trans.position + Vector3.back);
            //    if (adj != null && adj.IsSolid(eDirection.forward))
            //    {
            //        movement.z = Mathf.Min(movement.z, 1 - (trans.position.z % 1));
            //    }
            //}

            ////up
            //if (movement.y > 0)
            //{
            //    adj = world.GetBlock(trans.position + Vector3.up);
            //    if (adj != null && adj.IsSolid(eDirection.down))
            //    {
            //        movement.y = Mathf.Min(movement.y, 1 - (trans.position.y % 1));
            //    }
            //}
            ////down
            //else
            //{
            //    adj = world.GetBlock(trans.position + Vector3.down);
            //    if (adj != null && adj.IsSolid(eDirection.up))
            //    {
            //        movement.y = Mathf.Min(movement.y, 1 - (trans.position.y % 1));
            //    }
            //}
            //return true;
        }
    }
}
