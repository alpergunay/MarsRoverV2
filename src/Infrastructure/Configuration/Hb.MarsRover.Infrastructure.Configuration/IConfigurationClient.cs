using Hb.MarsRover.Infrastructure.Configuration.Settings;
using Hb.MarsRover.Infrastructure.Configuration.SettingsImplementation;
using System.Collections.Generic;

namespace Hb.MarsRover.Infrastructure.Configuration
{
    public interface IConfigurationClient
    {
        List<SettingsSection> SettingsSections { get; }

        T GetSettings<T>()
            where T : new();

        ApplicationSettings GetApplicationSettings();

        string GetConfigurationFolderPath();
    }
}