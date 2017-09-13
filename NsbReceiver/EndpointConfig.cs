using NsbExtensions;
using NsbMessages;
using NServiceBus;
using System;
using System.Configuration;

namespace NsbReceiver
{
    public class EndpointConfig : IConfigureThisEndpoint
    {
        private static readonly string SenderEndpointName = ConfigurationManager.AppSettings["SenderEndpointName"];
        private static readonly string ReceiverEndpointName = ConfigurationManager.AppSettings["ReceiverEndpointName"];
        private static readonly string ErrorQueue = ConfigurationManager.AppSettings["ErrorQueue"];
        private static readonly string AuditQueue = ConfigurationManager.AppSettings["AuditQueue"];
        private static readonly string InstanceMappingFile = ConfigurationManager.AppSettings["InstanceMappingFile"];

        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            endpointConfiguration
                .UniquelyIdentifyRunningInstance()
                .UsingNames(ReceiverEndpointName, Environment.MachineName);

            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.SendFailedMessagesTo(ErrorQueue);
            endpointConfiguration.AuditProcessedMessagesTo(AuditQueue);
            endpointConfiguration.EnableInstallers();
            
            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            transport.Transactions(TransportTransactionMode.TransactionScope);

            var routing = transport.Routing();
            routing.RegisterPublisher(typeof(EventMessage).Assembly, SenderEndpointName);
            routing.SetInstanceMappingFile(InstanceMappingFile);

            var sqlConnection = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
            endpointConfiguration.SetMsSqlPersistence(sqlConnection);
        }
    }
}
