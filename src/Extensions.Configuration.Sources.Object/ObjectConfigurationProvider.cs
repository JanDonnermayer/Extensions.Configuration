using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Extensions.Configuration.Sources.Object
{
    internal class ObjectConfigurationProvider : ConfigurationProvider
    {
        public ObjectConfigurationProvider(IEnumerable<KeyValuePair<string, object>> source)
        {
            foreach (var item in source.Fold(o => o.ToString()))
                Set(item.Key.Aggregate((x, y) => x + ":" + y), item.Value);
        }
    }
}
