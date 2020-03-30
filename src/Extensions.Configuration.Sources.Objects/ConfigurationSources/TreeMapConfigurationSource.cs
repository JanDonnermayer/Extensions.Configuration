using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Extensions.Configuration.Sources.Objects
{
    internal static class TreeMapConfigurationSource
    {
        public static IConfigurationSource From(IEnumerable<KeyValuePair<string, object>> treeEntries)
        {
            if (treeEntries is null)
                throw new System.ArgumentNullException(nameof(treeEntries));

            var entries = treeEntries
                .Fold(x => x.ToString())
                .Select(kvp =>
                    new KeyValuePair<string, string>(
                        kvp.Key.Aggregate((x, y) => x + ConfigurationPath.KeyDelimiter + y),
                        kvp.Value
                    )
                );

            return MapConfigurationSource.From(entries);
        }
    }
}
