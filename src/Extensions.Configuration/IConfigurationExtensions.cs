﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// Provides extension methods for <see cref="IConfiguration"/>.
    /// </summary>
    public static class IConfigurationExtensions
    {
        private const string PATTERN = @"\{\$env:([\s\S]*?)\}";

        /// <summary>
        /// Gets the string value associated to the specified <paramref name="key"/>,
        /// operating recursively on placeholders of format {$env:KEY}
        /// </summary>
        /// <remarks>
        /// Throws <see cref="InvalidOperationException"/> when encountering loops
        /// during the substitution process.
        /// </remarks>
        public static string ResolveValue(this IConfiguration configuration, string key)
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

                return Regex.Replace(
                    input: input,
                    pattern: PATTERN,
                    evaluator: m => resolveKey(
                        input: m.Groups[1].Value,
                        expressionPath: expressionPath.Add(input)
                    ),
                    options: RegexOptions.IgnoreCase
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
    }
}
