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

        private readonly Func<string, string> mapUnresolvable;

        /// <summary>
        /// Initiaizes a new instance of the <see cref="ResolverValueProvider"/>class.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="options"></param>
        /// <param name="mapUnresolvable">
        /// A function used to map placeholders, which could not be resolved.
        /// </param>
        public ResolverValueProvider(IValueProvider provider, SubstitutionOptions options, Func<string, string> mapUnresolvable)
        {
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
            this.options = options;
            this.mapUnresolvable = mapUnresolvable ?? throw new ArgumentNullException(nameof(mapUnresolvable));
        }

        /// <summary>
        /// Gets the value associated with the specified <paramref name="key"/>,
        /// recursively resolving placeholders.
        /// </summary>
        /// <param name="key">
        /// The key whose value to resolve.
        /// </param>
        public string GetValue(string key)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

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
