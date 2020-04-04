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
        /// <param name="mapUnresolvable">
        /// A function used to map placeholders, which could not be resolved.
        /// </param>
        /// <throws>
        /// Throws <see cref="ValueUnresolvableException"/> when encountering loops
        /// during the substitution process.
        /// </throws>
        public string GetValue(string key, Func<string, string> mapUnresolvable)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            if (mapUnresolvable is null)
                throw new ArgumentNullException(nameof(mapUnresolvable));

            string resolveExpression(string input, ImmutableHashSet<string> path)
            {
                if (path.Contains(input))
                {
                    throw new ValueUnresolvableException(
                        $"Encountered loop while trying to resolve expression: '{input}'."
                        + $"Path: ['{String.Join("' -> '", path)}']"
                    );
                }

                var nextPath = path.Add(input);

                return options
                    .ToRegexPatterns()
                    .Aggregate(input, (input, pattern) =>
                        Regex.Replace(
                            input: input,
                            pattern: pattern,
                            evaluator: m => resolveOrMap(m, nextPath) ,
                            options: RegexOptions.IgnoreCase
                        )
                    );
            }

            string resolveOrMap(Match match, ImmutableHashSet<string> path) =>
                provider.TryGetValue(match.Groups[1].Value, out var val)
                ? resolveExpression(val!, path)
                : mapUnresolvable(match.Value) ?? "";

            return resolveExpression(
                input: provider.GetValue(key),
                path: ImmutableHashSet<string>.Empty
            );
        }

        /// <summary>
        /// Gets the value associated with the specified <paramref name="key"/>,
        /// recursively resolving placeholders.
        /// </summary>
        /// <param name="key">
        /// The key whose value to resolve.
        /// </param>
        /// <throws>
        /// Throws <see cref="ValueUnresolvableException"/> when a value can
        /// not be resolved.
        /// </throws>
        public string GetValue(string key) =>
            GetValue(key, value => throw new ValueUnresolvableException($"Failed to resolve value: {value}"));

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
            catch (ValueUnresolvableException) { }

            value = null;
            return false;
        }
    }
}
