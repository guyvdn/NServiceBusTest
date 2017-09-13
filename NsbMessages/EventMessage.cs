using NServiceBus;

namespace NsbMessages
{
    public class EventMessage : IEvent
    {
        public string Data { get; set; }
    }
}
