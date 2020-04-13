using System;

namespace Microsoft.Extensions.Configuration
{
    internal static class IConfigurationProviderExtensions
    {
        /// <summary>
        /// Tries to get the configuration value for the specified key.
        /// </summary>
        /// <param name="provider">The <see cref="IConfigurationProvider"/> from which to get the value.</param>
        /// <param name="key">The key, whose value to get.</param>
        /// <returns>(true, value) if a value for the specified key was found, otherwise (false, null).</returns>
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
