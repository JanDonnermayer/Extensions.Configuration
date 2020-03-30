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

            return builder.Add(new GenericObjectConfigurationSource<T>(source));
        }

    }
}
