using Microsoft.Extensions.Configuration;

namespace Extensions.Configuration.Resolver
{
    internal static class IValueProviderExtensions
    {
        /// <summary>
        /// Creates an instance of <see cref="ResolverValueProvider"/> from the specified
        /// <paramref name="provider"/>.
        /// </summary>
        public static ResolverValueProvider ToResolverValueProvider(
            this IValueProvider provider, ResolverOptions options) =>
                new ResolverValueProvider(provider, options);
    }
}
