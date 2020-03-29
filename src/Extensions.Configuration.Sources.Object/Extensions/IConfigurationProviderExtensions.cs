using System;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.Configuration
{
    public static class IConfigurationProviderExtensions
    {
        public static (bool contains, string? value) TryGetValue(this IConfigurationProvider provider, string key)
        {
            if (provider is null)
                throw new ArgumentNullException(nameof(provider));

            if (key is null)
                throw new ArgumentNullException(nameof(key));

            return (provider.TryGet(key, out var value), value);
        }
    }
}
