using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace Extensions.Configuration
{
    public static class IConfigurationExtensions
    {
        private const string PATTERN = @"\{\$env:([\s\S]*)\}";

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
