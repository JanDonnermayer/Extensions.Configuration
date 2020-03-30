using System.Collections.Generic;
using System.Linq;
using Extensions.Configuration.Sources.Objects;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.Configuration
{
    public static class IConfigurationBuilderExtensions
    {
        /// <summary>
        /// Adds the properties of the specified <paramref name="source"/>
        /// as configuration entries to the <see cref="IConfigurationBuilder"/>,
        /// including nested objects.
        /// </summary>
        /// <param name="builder">The builder to which to add the configuration entries.</param>
        /// <param name="source">The object from which the properties are obtained.</param>
        /// <typeparam name="T">The type of the specified <paramref name="source"/></typeparam>
        /// <returns>The specified <see cref="IConfigurationBuilder"/> for chaining.</returns>
        /// <Example>
        /// builder.AddObject( ("key", "value") );
        /// <Example>
        public static IConfigurationBuilder AddObject<T>(this IConfigurationBuilder builder, T source)
        {
            if (builder == null)
                throw new System.ArgumentNullException(nameof(builder));

            return builder.Add(ObjectConfigurationSource.From(source));
        }

        public static IConfigurationBuilder AddEntry(this IConfigurationBuilder builder, string key, string value)
        {
            if (builder == null)
                throw new System.ArgumentNullException(nameof(builder));

            return builder.Add(MapSource(Of(KeyValuePair(key, value))));
        }

        public static IConfigurationBuilder AddEntry(this IConfigurationBuilder builder, (string key, string value) source)
        {
            if (builder == null)
                throw new System.ArgumentNullException(nameof(builder));

            return builder.Add(MapSource(Of(KeyValuePair(source))));
        }

        public static IConfigurationBuilder AddEntries(this IConfigurationBuilder builder, (string key, string value) source, params (string key, string value)[] sources)
        {
            if (builder == null)
                throw new System.ArgumentNullException(nameof(builder));

            return builder.Add(MapSource(KeyValuePairs(sources.Prepend(source))));
        }

        public static IConfigurationBuilder AddEntries(this IConfigurationBuilder builder, IEnumerable<(string key, string value)> sources)
        {
            if (builder == null)
                throw new System.ArgumentNullException(nameof(builder));

            return builder.Add(MapSource(KeyValuePairs(sources)));
        }

        private static KeyValuePair<string, string> KeyValuePair((string key, string value) source) =>
            KeyValuePair(source.key, source.value);

        private static KeyValuePair<string, string> KeyValuePair(string key, string value) =>
            new KeyValuePair<string, string>(key, value);

        private static IEnumerable<KeyValuePair<string, string>> KeyValuePairs(IEnumerable<(string key, string value)> sources) =>
            sources.Select(KeyValuePair);

        private static IConfigurationSource MapSource(IEnumerable<KeyValuePair<string, string>> source) =>
            MapConfigurationSource.From(source);

        private static IEnumerable<T> Of<T>(T element) => new[] { element };
    }
}
