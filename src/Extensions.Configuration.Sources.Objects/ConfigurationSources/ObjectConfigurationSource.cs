using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Extensions.Configuration.Sources.Objects
{
    internal static class ObjectConfigurationSource
    {
        public static IConfigurationSource FromObject<T>(T source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var entries = TreeMapProvider
                .GetTreeMap(source)
                .Fold(x => x.ToString())
                .Select(kvp =>
                    new KeyValuePair<string, string>(
                        kvp.Key.Aggregate((x, y) => x + ConfigurationPath.KeyDelimiter + y),
                        kvp.Value
                    )
                );

            return MapConfigurationSource.FromEntries(entries);
        }
    }
}
