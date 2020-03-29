using Microsoft.Extensions.Configuration;

namespace Extensions.Configuration.Sources.Objects
{
    internal static class ObjectConfigurationSource
    {
        public static IConfigurationSource From<T>(T source) =>
            new GenericObjectConfigurationSource<T>(source);
    }
}
