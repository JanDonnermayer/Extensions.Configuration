using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace Extensions.Configuration.Resolver
{
    internal class ResolverValueProvider : IValueProvider
    {
        private readonly IValueProvider provider;
        private readonly SubstitutionOptions options;

        public ResolverValueProvider(IValueProvider provider, SubstitutionOptions options)
        {
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
            this.options = options;
        }

        /// <summary>
        /// Gets the value associated with the specified <paramref name="key"/>,
        /// recursively resolving placeholders.
        /// </summary>
        /// <param name="key">
        /// The key whose value to resolve.
        /// </param>
        /// <throws>
        /// Throws <see cref="InvalidOperationException"/> when encountering loops
        /// during the substitution process.
        /// </throws>
        public string GetValue(string key)
        {
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
               resolveExpression(provider.GetValue(input), expressionPath);

            return resolveKey(key, ImmutableHashSet<string>.Empty);
        }

        /// <summary>
        /// Gets the value associated with the specified <paramref name="key"/>,
        /// recursively resolving placeholders.
        /// </summary>
        /// <param name="key">
        /// The key whose value to resolve.
        /// </param>
        /// <param name="value">
        /// When this method returns, the value associated with the specified key, 
        /// if the key can be resolved; otherwise, null.
        /// This parameter is passed uninitialized.
        /// </param>
        public bool TryGetValue(string key, out string? value)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            try
            {
                value = GetValue(key);
                return true;
            }
            catch (KeyNotFoundException) { }
            catch (InvalidOperationException) { }

            value = null;
            return false;
        }
    }
}
