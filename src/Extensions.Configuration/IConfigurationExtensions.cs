using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace Extensions.Configuration
{
    /// <summary>
    /// Provides extension methods for <see cref="IConfiguration"/>
    /// </summary>
    public static class IConfigurationExtensions
    {
        private const string PATTERN = @"\{\$env:([\s\S]*)\}";

        /// <summary>
        /// Gets the string value associated to the specified <paramref name="key" />,
        /// operating recursively on placeholders of format {$env:<KEY>}
        /// </summary>
        /// <remarks>
        /// Throws <see cref="InvalidOperationException"> when encountering loops
        /// during the substitution process.
        /// </remarks>
        public static string ResolveValue(this IConfiguration configuration, string key)
        {
            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));

            if (key is null)
                throw new ArgumentNullException(nameof(key));

            var encounteredExpressions = new HashSet<string>();

            string resolveExpression(string input)
            {
                if (!encounteredExpressions.Add(input))
                    throw new InvalidOperationException("Encountered loop!");

                return Regex.Replace(
                    input: input,
                    pattern: PATTERN,
                    evaluator: m => resolveKey(m.Groups[1].Value)
                );
            }

            string resolveKey(string input) =>
                configuration[input] switch
                {
                    String value => resolveExpression(value),
                    _ => input
                };

            return resolveKey(key);
        }
    }
}
