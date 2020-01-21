﻿using System;
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
            SubstitutionFormatOptions options =
                SubstitutionFormatOptions.CurlyBracketsDollarEnv
                | SubstitutionFormatOptions.DollarBrackets
                | SubstitutionFormatOptions.DollarCurlyBrackets
                | SubstitutionFormatOptions.Percent
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
        /// Creates an instance of <see cref="IConfiguration"/>,
        /// that accesses the specified instance <paranref name="configuration"/>,
        /// recursively resolving placeholders of formats specified in <paramref name="options"/>
        /// </summary>
        /// <param name="options">
        /// The formats recognized as placeholders.
        /// </param>
        public static IConfiguration Resolved(
            this IConfiguration configuration,
            SubstitutionFormatOptions options =
                SubstitutionFormatOptions.CurlyBracketsDollarEnv
                | SubstitutionFormatOptions.DollarBrackets
                | SubstitutionFormatOptions.DollarCurlyBrackets
                | SubstitutionFormatOptions.Percent
        )
        {
            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));

            static string? resolveValue(
                IConfiguration config, string key, SubstitutionFormatOptions options
            )
            {
                try
                {
                    return ResolveValue(
                        configuration: config,
                        key: key,
                        options: options
                    );
                }
                catch (InvalidOperationException)
                {
                    // required by implicit contract
                    return null;
                }
                catch (KeyNotFoundException)
                {
                    // required by implicit contract
                    return null;
                }
            }

            return new ConfigurationProxy(
                configuration: configuration,
                valueProvider: (config, key) => resolveValue(
                    config: config,
                    key: key,
                    options: options
                )! // required by signature
            );
        }
    }
}
