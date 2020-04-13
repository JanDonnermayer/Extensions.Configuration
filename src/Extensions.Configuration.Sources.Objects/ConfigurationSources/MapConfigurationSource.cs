using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Extensions.Configuration.Sources.Objects
{
    internal static class MapConfigurationSource
    {
        public static IConfigurationSource FromEntries(IEnumerable<KeyValuePair<string, string>> entries)
        {
            if (entries is null)
                throw new System.ArgumentNullException(nameof(entries));

            var provider = MapConfigurationProvider.FromEntries(entries);

            return ConfigurationSource.FromProvider(provider);
        }
    }
}
