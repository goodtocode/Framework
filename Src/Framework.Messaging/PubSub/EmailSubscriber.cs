using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodToCode.Framework.PubSub
{
    /// <summary>
    /// Email Listener interface
    /// </summary>
    public interface IEmailListener : ISubscriberService
    { }

    /// <summary>
    /// Email Listener event
    /// </summary>
    public class EmailListener : IEmailListener
    {
        /// <summary>
        /// Types of email events
        /// </summary>
        public IEnumerable<Type> EventTypes
        {
            get
            {
                return new List<Type>()
                {
                    typeof(EnrollmentPromotionEvent),
                    typeof(EnrollmentCreationEvent)
                };
            }
        }

        /// <summary>
        /// Handles the event
        /// </summary>
        /// <param name="Event"></param>
        /// <returns></returns>
        public async Task HandleEvent(IEvent Event)
        {
            var a = 23; // Email something
        }
    }
}
