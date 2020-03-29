using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Extensions.Configuration.Sources.Object
{
    internal class ObjectConfigurationProvider : ConfigurationProvider
    {
        public ObjectConfigurationProvider(IEnumerable<KeyValuePair<IEnumerable<string>, string>> source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            foreach (var item in source)
                Set(item.Key.Aggregate((x, y) => x + ":" + y), item.Value);
        }

        public static IConfigurationProvider From(IEnumerable<KeyValuePair<IEnumerable<string>, string>> source) =>
            new ObjectConfigurationProvider(source);
    }
}
