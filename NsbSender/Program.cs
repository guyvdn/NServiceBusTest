using System;
using System.Configuration;
using System.Threading.Tasks;
using NsbMessages;
using NServiceBus;
using NsbExtensions;

namespace NsbSender
{
    public class Program
    {
        private static readonly string SenderEndpointName = ConfigurationManager.AppSettings["SenderEndpointName"];
        private static readonly string ReceiverEndpointName = ConfigurationManager.AppSettings["ReceiverEndpointName"];
        private static readonly string ErrorQueue = ConfigurationManager.AppSettings["ErrorQueue"];
        private static readonly string AuditQueue = ConfigurationManager.AppSettings["AuditQueue"];

        private static void Main()
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        private static async Task AsyncMain()
        {
            var endpointConfiguration = ConfigureEndpoint();

            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            var input = char.MinValue;
            while (input != 'x')
            {
                Console.WriteLine("Press c to send a Command");
                Console.WriteLine("Press e to publish an Event");
                Console.WriteLine("Press x to Exit");

                input = Console.ReadKey().KeyChar;

                if (input == 'c')
                {
                    await endpointInstance.Send<CommandMessage>(ReceiverEndpointName, m => m.Data = $"This is a command sent at {DateTime.Now}");
                    Console.WriteLine("Command sent");
                }

                if (input == 'e')
                {
                    await endpointInstance.Publish<EventMessage>(m => m.Data = $"This is an Event published at {DateTime.Now}");
                    Console.WriteLine("Event published");
                }
            }

            await endpointInstance.Stop().ConfigureAwait(false);
        }

        private static EndpointConfiguration ConfigureEndpoint()
        {
            var endpointConfiguration = new EndpointConfiguration(SenderEndpointName);

            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.SendFailedMessagesTo(ErrorQueue);
            endpointConfiguration.AuditProcessedMessagesTo(AuditQueue);
            endpointConfiguration.EnableInstallers();

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            transport.Transactions(TransportTransactionMode.TransactionScope);

            var sqlConnection = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
            endpointConfiguration.SetMsSqlPersistence(sqlConnection);

            return endpointConfiguration;
        }
    }
}
