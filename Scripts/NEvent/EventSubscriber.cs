using System;

namespace Assets.Scripts.NEvent
{
    public interface IEventSubscriber
    {
        void Subscribe(Guid EventID, Del_HandleEvent handler, byte Option = SubsribeOption.NO_OPTION);
        void Subscribe_Deccorate(Guid EventID, Del_DecorateEvent decorator, byte Option = SubsribeOption.NO_OPTION);
    }

    class EventSubscriber : IEventSubscriber
    {
        private IEveneCenter m_refEventCenter;

        //Constructor
        public EventSubscriber(IEveneCenter EventCenter)
        {
            m_refEventCenter = EventCenter;
        }

        //subscribe
        public void Subscribe(Guid EventID, Del_HandleEvent handler, byte Option = SubsribeOption.NO_OPTION)
        {
            m_refEventCenter.SubScribe(EventID, handler, Option);
        }
        public void Subscribe_Deccorate(Guid EventID, Del_DecorateEvent decorator, byte Option = SubsribeOption.NO_OPTION)
        {
            m_refEventCenter.SubScribe_Decorate(EventID, decorator, Option);
        }
    }
}
