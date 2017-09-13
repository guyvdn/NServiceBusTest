using System;
using System.Threading.Tasks;
using NsbMessages;
using NServiceBus;

namespace NsbReceiver
{
    public class EventHandler : IHandleMessages<EventMessage>
    { 
        public Task Handle(EventMessage message, IMessageHandlerContext context)
        {
            return Task.Run(() => Console.WriteLine("Event received: " + message.Data));
        }
    }
}