using NServiceBus;
using System;

namespace NsbExtensions
{
    public static class RoutingExtensions
    {
        public static RoutingSettings<MsmqTransport> SetInstanceMappingFile(this RoutingSettings<MsmqTransport> routing, string filePath)
        {
            var instanceMappingFile = routing.InstanceMappingFile();
            var fileSettings = instanceMappingFile.FilePath(filePath);
            fileSettings.RefreshInterval(TimeSpan.FromMinutes(1));

            return routing;
        }
    }
}
