namespace GoodToCode.Entity.PubSub
{
    public interface IPubSubService
    {
        void Subscribe(ISubscriberService subscriber);
        void Publish(IEvent Event);
    }
}
