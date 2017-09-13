using NServiceBus;
using NServiceBus.Persistence.Sql;
using System;
using System.Data.SqlClient;

namespace NsbExtensions
{
    public static class EndpointConfigurationExtensions
    {
        public static EndpointConfiguration SetMsSqlPersistence(
            this EndpointConfiguration endpointConfiguration,
            string connectionString,
            string tablePrefix = "NServiceBus",
            int cacheInMinutes = 10
            )
        {
            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            persistence.SqlVariant(SqlVariant.MsSqlServer);
            persistence.ConnectionBuilder(() => new SqlConnection(connectionString));
            persistence.TablePrefix(tablePrefix);

            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.CacheFor(TimeSpan.FromMinutes(cacheInMinutes));

            return endpointConfiguration;
        }
    }
}