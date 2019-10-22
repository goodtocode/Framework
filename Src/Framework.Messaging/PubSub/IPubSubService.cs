namespace GoodToCode.Framework.PubSub
{
    public interface IPubSubService
    {
        void Subscribe(ISubscriberService subscriber);
        void Publish(IEvent Event);
    }
}
