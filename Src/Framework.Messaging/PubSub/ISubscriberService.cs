using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodToCode.Framework.PubSub
{

    public interface ISubscriberService
    {
        IEnumerable<Type> EventTypes { get; }
        Task HandleEvent(IEvent Event);
    }    
}
