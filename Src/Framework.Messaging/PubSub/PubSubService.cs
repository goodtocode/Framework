using System.Collections.Generic;
using System.Linq;

namespace GoodToCode.Framework.PubSub
{
    //Startup.cs
    //            services.AddScoped<IPubSubService>(sp => {
    //    var emailListener = sp.GetService<IEmailListener>();
    //    var loggerListener = sp.GetService<ILoggerListener>();
    //    var bus = new PubSubService();
    //    bus.Subscribe(subscriber: emailListener);
    //    bus.Subscribe(subscriber: loggerListener);
    //    return bus;
    //});
    //        services.AddScoped<IEmailListener, EmailListener>();
    //        services.AddScoped<ILoggerListener, LoggerListener>();

    // Controller
    //    private IPubSubService _PubSubService;

    //public HomeController(IPubSubService PubSubService)
    //{
    //    _PubSubService = PubSubService;
    //}

    //[Route("/")]
    //public IActionResult Index()
    //{
    //    _PubSubService.Publish(Event: new EnrollmentCreationEvent());
    //    return new ContentResult(); // ViewComponent(typeof(TestViewComponent));
    //}

    public class PubSubService : IPubSubService
    {
        private List<ISubscriberService> _subscribers = new List<ISubscriberService>();

        public void Subscribe(ISubscriberService subscriber)
        {
            _subscribers.Add(item: subscriber);
        }
        public void Publish(IEvent Event)
        {
            var subscribers = _subscribers.Where(s => s.EventTypes.Contains(Event.GetType())).Select(s => s).ToList();
            foreach (ISubscriberService subscriber in subscribers)
            {
                subscriber.HandleEvent(Event);
            }
        }
    }
}
