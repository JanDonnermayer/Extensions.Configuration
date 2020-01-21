using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Configuration
{
    internal class ConfigurationResolverProxy : IConfiguration
    {
        private readonly IConfiguration configuration;
        private readonly Func<IConfiguration, string, string> valueProvider;

        public ConfigurationResolverProxy(
            IConfiguration configuration,
            Func<IConfiguration, string, string> valueProvider
        )
        {
            this.configuration = configuration
                ?? throw new ArgumentNullException(nameof(configuration));
            this.valueProvider = valueProvider
                ?? throw new ArgumentNullException(nameof(valueProvider));
        }

        string IConfiguration.this[string key]
        {
            get => valueProvider(configuration, key);
            set => configuration[key] = value;
        }

        IEnumerable<IConfigurationSection> IConfiguration.GetChildren() =>
            configuration.GetChildren();

        IChangeToken IConfiguration.GetReloadToken() =>
            configuration.GetReloadToken();

        IConfigurationSection IConfiguration.GetSection(string key) =>
            configuration.GetSection(key);
    }
}