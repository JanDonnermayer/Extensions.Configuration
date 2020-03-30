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
        public static IConfigurationBuilder AddObject<T>(
            this IConfigurationBuilder builder,
            T source
        )
        {
            if (builder == null)
                throw new System.ArgumentNullException(nameof(builder));

            return builder.Add(ObjectConfigurationSource.Of(source));
        }

        public static IConfigurationBuilder AddEntry(
            this IConfigurationBuilder builder,
            string key,
            string value
        )
        {
            if (builder == null)
                throw new System.ArgumentNullException(nameof(builder));

            return builder.Add(MapSource(Of(KeyValuePair(key, value))));
        }

        public static IConfigurationBuilder AddEntries(
            this IConfigurationBuilder builder,
            IEnumerable<(string, string)> entries
        )
        {
            if (builder == null)
                throw new System.ArgumentNullException(nameof(builder));

            if (entries == null)
                throw new System.ArgumentNullException(nameof(entries));

            return builder.Add(MapSource(KeyValuePairs(entries)));
        }

        public static IConfigurationBuilder AddEntries(
            this IConfigurationBuilder builder,
            (string, string) entry,
            params (string, string)[] entries
        )
        {
            if (builder == null)
                throw new System.ArgumentNullException(nameof(builder));

            return builder.AddEntries(Of(entry).Concat(entries));
        }

        private static KeyValuePair<T, V> KeyValuePair<T, V>(T key, V value) =>
            new KeyValuePair<T, V>(key, value);

        private static KeyValuePair<T, V> KeyValuePair<T, V>((T key, V value) entry) =>
            KeyValuePair(entry.key, entry.value);

        private static IEnumerable<KeyValuePair<T, V>> KeyValuePairs<T, V>(IEnumerable<(T key, V value)> entries) =>
            entries.Select(KeyValuePair<T, V>);

        private static IConfigurationSource MapSource(IEnumerable<KeyValuePair<string, string>> entries) =>
            MapConfigurationSource.Of(entries);

        private static IEnumerable<T> Of<T>(T element) => new[] { element };
    }
}
