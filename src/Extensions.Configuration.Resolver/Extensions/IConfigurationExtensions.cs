using System;
using Extensions.Configuration.Resolver;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// Provides extension methods for <see cref="IConfiguration"/>.
    /// </summary>
    public static class IConfigurationExtensions
    {
        /// <summary>
        /// Creates an instance of <see cref="ConfigurationValueProvider"/> from the specified
        /// <paramref name="configuration"/>.
        /// </summary>
        internal static ConfigurationValueProvider ToConfigurationValueProvider(
            this IConfiguration configuration) =>
                new ConfigurationValueProvider(configuration);

        /// <summary>
        /// Gets the value associated with the specified <paramref name="key"/>,
        /// recursively resolving placeholders according to specified <paramref name="options"/>
        /// </summary>
        /// <param name="configuration">
        /// The <paramref name="configuration"/> providing the values.
        /// </param>
        /// <param name="key">
        /// The key whose value to resolve.
        /// </param>
        /// <param name="options">
        /// The options used in the resolver process.
        /// </param>
        public static string ResolveValue(
            this IConfiguration configuration, string key,
            ResolverOptions options = ResolverOptions.All) =>
                configuration
                    .ToConfigurationValueProvider()
                    .ToResolverValueProvider(options)
                    .GetValue(key);

        /// <summary>
        /// Gets the value associated with the specified <paramref name="key"/>,
        /// recursively resolving placeholders according to specified <paramref name="options"/>.
        /// </summary>
        /// <param name="configuration">
        /// The <paramref name="configuration"/> providing the values.
        /// </param>
        /// <param name="key">
        /// The key whose value to resolve.
        /// </param>
        /// <param name="value">
        /// When this method returns, the value associated with the specified key,
        /// if the key can be resolved; otherwise, null.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <param name="options">
        /// The options used in the resolver process.
        /// </param>
        public static bool TryResolveValue(this IConfiguration configuration, string key,
            out string? value, ResolverOptions options = ResolverOptions.All) =>
                configuration
                    .ToConfigurationValueProvider()
                    .ToResolverValueProvider(options)
                    .TryGetValue(key, out value);

        /// <summary>
        /// Creates an instance of <see cref="IConfiguration"/>, wherein
        /// entries with placeholders are resolved according to specified <paramref name="options"/>.
        /// <param name="configuration">
        /// The <paramref name="configuration"/> providing the values.
        /// </param>
        /// </summary>
        /// <param name="options">
        /// The options used in the resolver process.
        /// </param>
        public static IConfiguration Resolved(
            this IConfiguration configuration,
            ResolverOptions options = ResolverOptions.All
        )
        {
            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));

            return new ConfigurationProxy(
                configuration: configuration,
                valueProvider: (config, key) => TryResolveValue(
                    configuration: config,
                    key: key,
                    value: out var val,
                    options: options
                ) ? val! : null! // Required by signature
            );
        }
    }
}
