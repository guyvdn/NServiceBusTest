using System;
using System.Threading.Tasks;
using NsbMessages;
using NServiceBus;

namespace NsbReceiver
{
    public class CommandHandler : IHandleMessages<CommandMessage>
    {
        public Task Handle(CommandMessage message, IMessageHandlerContext context)
        {
            return Task.Run(() => Console.WriteLine("Command received: " + message.Data));
        }
    }
}
