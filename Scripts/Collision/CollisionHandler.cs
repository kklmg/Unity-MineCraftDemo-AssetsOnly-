using Assets.Scripts.EventManager;
using Assets.Scripts.CharacterSpace;
using Assets.Scripts.Pattern;
using Assets.Scripts.WorldComponent;

namespace Assets.Scripts.Collision
{
    class CollisionHandler : IEventHandler
    {
        void SubscribeEvents()
        {
            ServiceLocator<IEventSubscriber>.GetService().
                Subscribe(Unique<Event_Character_TryMove>.ID,this);
        }


        private Event_Character_TryMove cache;
        public bool Handle(IEvent _event)
        {
            //ServiceLocator<IWorld>.GetService().Chunk_Height;

            cache = _event as Event_Character_TryMove;
            return true;

        }
         
        
    }
}
