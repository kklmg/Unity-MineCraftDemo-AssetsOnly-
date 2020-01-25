


namespace Assets.Scripts.NEvent
{
    public interface IEventPublisher
    {
        void Publish(IEvent _event);

        //publish the event and handle this immediately
        void PublishAndHandle(IEvent _event);
    }
    public class EventPublisher : IEventPublisher
    {
        private IEveneCenter m_refEventCenter;

        public EventPublisher(IEveneCenter EventMng)
        {
            m_refEventCenter = EventMng;
        }

        public void Publish(IEvent _event)
        {
            m_refEventCenter.GetEvent(_event);
        }
        public void PublishAndHandle(IEvent _event)
        {
            m_refEventCenter.Handle(_event);
        }

    }
}
