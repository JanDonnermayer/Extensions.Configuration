using System;
using Microsoft.Extensions.Configuration;

namespace Extensions.Configuration.Sources.Objects
{
    internal static class ObjectConfigurationSource
    {
        public static IConfigurationSource From<T>(T source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return TreeMapConfigurationSource.From(TreeMapProvider.GetTreeMap(source));
        }
    }
}
