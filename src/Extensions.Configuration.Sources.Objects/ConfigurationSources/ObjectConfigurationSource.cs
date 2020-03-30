using System;
using Microsoft.Extensions.Configuration;

namespace Extensions.Configuration.Sources.Objects
{
    internal static class ObjectConfigurationSource
    {
        public static IConfigurationSource Of<T>(T source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var treeMap = TreeMapProvider.GetTreeMap(source);

            return TreeMapConfigurationSource.Of(treeMap);
        }
    }
}
