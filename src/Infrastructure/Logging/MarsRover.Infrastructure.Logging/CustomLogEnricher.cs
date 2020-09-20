using Serilog.Core;
using Serilog.Events;

namespace Hb.MarsRover.Infrastructure.Logging
{
    internal sealed class CustomLogEnricher : ILogEventEnricher
    {
        private LogEventProperty cachedProperty;

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            cachedProperty ??= propertyFactory.CreateProperty("CustomPropertyName", "CustomPropertyValue");
            logEvent.AddPropertyIfAbsent(cachedProperty);
        }
    }
}