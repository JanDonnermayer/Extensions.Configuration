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
        /// <param name="mapUnresolvable">
        /// A function used to map placeholders, which could not be resolved.
        /// </param>
        public static string ResolveValue(
            this IConfiguration configuration, string key,
            SubstitutionOptions options, Func<string, string> mapUnresolvable) =>
                configuration
                    .ToConfigurationValueProvider()
                    .ToResolverValueProvider(options, mapUnresolvable)
                    .GetValue(key);

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
        /// <throws>
        /// Throws <see cref="ValueUnresolvableException"/> when a value can
        /// not be resolved.
        /// </throws>
        public static string ResolveValue(
            this IConfiguration configuration, string key,
            SubstitutionOptions options = SubstitutionOptions.All) =>
                configuration
                    .ToConfigurationValueProvider()
                    .ToResolverValueProvider(options, ThrowValueUnresolvableException)
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
            out string? value, SubstitutionOptions options = SubstitutionOptions.All) =>
                configuration
                    .ToConfigurationValueProvider()
                    .ToResolverValueProvider(options, ThrowValueUnresolvableException)
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
            SubstitutionOptions options = SubstitutionOptions.All
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

        private static string ThrowValueUnresolvableException(string value) =>
            throw new ValueUnresolvableException($"Failed to resolve value: {value}");
    }
}
