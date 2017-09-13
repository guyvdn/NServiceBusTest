using NServiceBus;

namespace NsbMessages
{
    public class CommandMessage : ICommand
    {
        public string Data { get; set; }
    }
}
