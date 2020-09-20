using Serilog.Core;
using Serilog.Events;

namespace Hb.MarsRover.Infrastructure.Logging
{
    internal class PropertyEnricher : ILogEventEnricher
    {
        private readonly string propertyName;
        private readonly string propertyValue;
        private LogEventProperty cachedProperty;

        public PropertyEnricher(string propertyName, string propertyValue)
        {
            this.propertyName = propertyName;
            this.propertyValue = propertyValue;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            cachedProperty = cachedProperty ?? propertyFactory.CreateProperty(propertyName, propertyValue);
            logEvent.AddPropertyIfAbsent(cachedProperty);
        }
    }
}