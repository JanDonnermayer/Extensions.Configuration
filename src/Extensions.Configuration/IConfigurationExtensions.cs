using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// Provides extension methods for <see cref="IConfiguration"/>.
    /// </summary>
    public static class IConfigurationExtensions
    {
        /// <summary>
        /// Gets the string value associated to the specified <paramref name="key"/>,
        /// recursively resolving placeholders of formats specified in <paramref name="options"/>
        /// </summary>
        /// <param name="options">
        /// The formats recognized as placeholders.
        /// </param>
        /// <throws> 
        /// Throws <see cref="InvalidOperationException"/> when encountering loops
        /// during the substitution process.
        /// Throws <see cref="KeyNotFoundException"/> when a key is not found.
        /// </throws>
        public static string ResolveValue(
            this IConfiguration configuration, string key,
            SubstitutionFormatOptions options = SubstitutionFormatOptions.All
        )
        {
            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));

            if (key is null)
                throw new ArgumentNullException(nameof(key));

            string resolveExpression(string input, ImmutableHashSet<string> expressionPath)
            {
                if (expressionPath.Contains(input))
                {
                    throw new InvalidOperationException(
                        $"Encountered loop while trying to resolve expression: '{input}'."
                        + $"Path: ['{String.Join("' -> '", expressionPath)}']"
                    );
                }

                var nextExpresionPath = expressionPath.Add(input);

                return options
                    .ToRegexPatterns()
                    .Aggregate(input, (input, pattern) =>
                    {
                        return Regex.Replace(
                            input: input,
                            pattern: pattern,
                            evaluator: m => resolveKey(
                                input: m.Groups[1].Value,
                                expressionPath: nextExpresionPath
                            ),
                            options: RegexOptions.IgnoreCase
                        );
                    }
                );
            }

            string resolveKey(string input, ImmutableHashSet<string> expressionPath) =>
               configuration[input] switch
               {
                   String value => resolveExpression(value, expressionPath),
                   _ => throw new KeyNotFoundException($"No such key: '{input}'")
               };

            return resolveKey(key, ImmutableHashSet<string>.Empty);
        }

        /// <summary>
        /// Tries to the string value associated to the specified <paramref name="key"/>,
        /// recursively resolving placeholders of formats specified in <paramref name="options"/>.
        /// If found returns true, else: false.
        /// </summary>
        /// <param name="options">
        /// The formats recognized as placeholders.
        /// </param>
        public static bool TryResolveValue(this IConfiguration configuration, string key,
            out string? value, SubstitutionFormatOptions options = SubstitutionFormatOptions.All
        )
        {
            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));

                try
                {
                    value = ResolveValue(
                        configuration: configuration,
                        key: key,
                        options: options
                    );
                    return true;
                }
                catch (InvalidOperationException)
                {
                }
                catch (KeyNotFoundException)
                {
                }
                value = null;
                return false;
        }

        /// <summary>
        /// Creates an instance of <see cref="IConfiguration"/>,
        /// that accesses the specified instance <paranref name="configuration"/>,
        /// recursively resolving placeholders of formats specified in <paramref name="options"/>
        /// </summary>
        /// <param name="options">
        /// The formats recognized as placeholders.
        /// </param>
        public static IConfiguration Resolved(
            this IConfiguration configuration,
            SubstitutionFormatOptions options = SubstitutionFormatOptions.All
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
