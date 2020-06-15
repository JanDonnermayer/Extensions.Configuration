using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Configuration
{
    internal static class ResolverOptionsExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="options"/> to their corresponding
        /// Regular Expressions.
        /// </summary>
        public static IEnumerable<string> ToRegexPatterns(this SubstitutionOptions options)
        {
            if ((options & SubstitutionOptions.CurlyBracketsDollarEnv) != 0)
                yield return @"\{\$env:([\s\S]*?)\}";

            if ((options & SubstitutionOptions.DollarCurlyBrackets) != 0)
                yield return @"\$\{([\s\S]*?)\}";

            if ((options & SubstitutionOptions.DollarBrackets) != 0)
                yield return @"\$\(([\s\S]*?)\)";

            if ((options & SubstitutionOptions.Percent) != 0)
                yield return @"\%([\s\S]*?)\%";
        }
    }
}
