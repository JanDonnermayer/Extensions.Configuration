using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Extensions.Configuration.Resolver
{
    internal class ConfigurationValueProvider : IValueProvider
    {
        private readonly IConfiguration configuration;

        public ConfigurationValueProvider(IConfiguration configuration)
        {
            this.configuration = configuration
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string GetValue(string key) =>
            configuration[key] switch
            {
                String value => value,
                _ => throw new KeyNotFoundException($"No such key: '{key}'")
            };

        public (bool success, string? value) TryGetValue(string key) =>
            configuration[key] switch
            {
                string val => (true, val),
                _ => (false, null)
            };
    }
}
