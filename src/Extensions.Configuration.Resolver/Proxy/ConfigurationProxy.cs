using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Extensions.Configuration.Resolver
{
    internal class ConfigurationProxy : IConfiguration
    {
        private readonly IConfiguration configuration;
        private readonly Func<IConfiguration, string, string> valueProvider;

        public ConfigurationProxy(
            IConfiguration configuration,
            Func<IConfiguration, string, string> valueProvider
        )
        {
            this.configuration = configuration
                ?? throw new ArgumentNullException(nameof(configuration));
            this.valueProvider = valueProvider
                ?? throw new ArgumentNullException(nameof(valueProvider));
        }

        public string this[string key]
        {
            get => valueProvider(configuration, key);
            set => configuration[key] = value;
        }

        public IEnumerable<IConfigurationSection> GetChildren() =>
            configuration.GetChildren();

        public IChangeToken GetReloadToken() =>
            configuration.GetReloadToken();

        public IConfigurationSection GetSection(string key) =>
            new ConfigurationSectionProxy(configuration.GetSection(key), valueProvider);
    }
}