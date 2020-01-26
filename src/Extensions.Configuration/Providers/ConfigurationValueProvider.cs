using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Configuration
{
    internal class ConfigurationValueProvider : IValueProvider
    {
        private readonly IConfiguration configuration;

        public ConfigurationValueProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GetValue(string key) =>
            configuration[key] switch
            {
                String value => value,
                _ => throw new KeyNotFoundException($"No such key: '{key}'")
            };

        public bool TryGetValue(string key, out string? value)
        {
            try
            {
                value = GetValue(key);
                return true;
            }
            catch (KeyNotFoundException) { }

            value = null;
            return false;
        }
    }

}
