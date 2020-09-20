using System;

namespace Hb.MarsRover.Infrastructure.Configuration.SettingsImplementation
{
    public sealed class Setting
    {
        public Setting(string name, string value)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}