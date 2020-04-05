using System;
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
            this IValueProvider provider, SubstitutionOptions options, Func<string, string> mapUnresolvable) =>
                new ResolverValueProvider(provider, options, mapUnresolvable);
    }
}
