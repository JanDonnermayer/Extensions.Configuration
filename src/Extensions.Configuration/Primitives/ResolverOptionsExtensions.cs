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
        public static IEnumerable<string> ToRegexPatterns(this ResolverOptions options)
        {
            if ((options & ResolverOptions.CurlyBracketsDollarEnv) != 0)
                yield return @"\{\$env:([\s\S]*?)\}";

            if ((options & ResolverOptions.DollarCurlyBrackets) != 0)
                yield return @"\$\{([\s\S]*?)\}";

            if ((options & ResolverOptions.DollarBrackets) != 0)
                yield return @"\$\(([\s\S]*?)\)";

            if ((options & ResolverOptions.Percent) != 0)
                yield return @"\%([\s\S]*?)\%";
        }
    }
}
