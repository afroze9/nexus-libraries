﻿using Microsoft.Extensions.Configuration;
using Winton.Extensions.Configuration.Consul;

namespace Nexus.Configuration;

public static class ConfigurationExtensions
{
    private static void AddConsulKv(this ConfigurationManager configuration, ConsulKVSettings settings)
    {
        configuration.AddConsul(settings.Key, options =>
        {
            options.ConsulConfigurationOptions = config =>
            {
                config.Address = new Uri(settings.Url);
                config.Token = settings.Token;
            };

            options.Optional = false;
            options.ReloadOnChange = true;
        });
    }

    public static void AddCoreConfiguration(this ConfigurationManager configuration)
    {
        configuration.SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables();

        // Settings for consul kv
        ConsulKVSettings consulKvSettings = new ();
        configuration.GetRequiredSection("ConsulKV").Bind(consulKvSettings);
        configuration.AddConsulKv(consulKvSettings);
    }
}