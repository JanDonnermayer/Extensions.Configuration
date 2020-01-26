using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Extensions.Configuration.Resolver
{
    internal class ConfigurationSectionProxy : IConfigurationSection
    {
        private readonly IConfigurationSection configurationSection;
        private readonly Func<IConfiguration, string, string> valueProvider;

        public ConfigurationSectionProxy(
            IConfigurationSection configurationSection,
            Func<IConfiguration, string, string> valueProvider)
        {
            this.configurationSection = configurationSection
                ?? throw new ArgumentNullException(nameof(configurationSection));
            this.valueProvider = valueProvider
                ?? throw new ArgumentNullException(nameof(valueProvider));
        }

        public string this[string key]
        {
            get => valueProvider(configurationSection, key);
            set => configurationSection[key] = value;
        }

        public string Key => configurationSection.Key;

        public string Path => configurationSection.Path;

        public string Value
        {
            get => configurationSection.Value;
            set => configurationSection.Value = value;
        }

        public IEnumerable<IConfigurationSection> GetChildren() =>
            configurationSection.GetChildren();

        public IChangeToken GetReloadToken() =>
            configurationSection.GetReloadToken();

        public IConfigurationSection GetSection(string key) =>
            new ConfigurationSectionProxy(configurationSection.GetSection(key), valueProvider);
    }
}