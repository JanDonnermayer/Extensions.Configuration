using System;
using Microsoft.Extensions.Configuration;

namespace Extensions.Configuration.Builder.Objects
{
    public static class ConfigurationBuilderExtensions
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
        public static IConfigurationBuilder AddObject<T>(this IConfigurationBuilder builder, T source)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            foreach (var kvp in DictionaryConverter.GetDictionary(source))
                builder.Properties[kvp.Key] = kvp.Value;

            return builder;
        }
    }
}
