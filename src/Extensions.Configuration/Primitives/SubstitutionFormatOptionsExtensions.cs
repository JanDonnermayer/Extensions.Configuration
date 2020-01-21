using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Configuration
{
    internal static class SubstitutionFormatOptionsExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="options"/> to their corresponding 
        /// Regular Expressions.
        /// </summary>
        public static IEnumerable<string> ToRegexPatterns(this SubstitutionFormatOptions options)
        {
            if ((options & SubstitutionFormatOptions.CurlyBracketsDollarEnv) != 0)
                yield return @"\{\$env:([\s\S]*?)\}";

            if ((options & SubstitutionFormatOptions.DollarCurlyBrackets) != 0)
                yield return @"\$\{([\s\S]*?)\}";

            if ((options & SubstitutionFormatOptions.DollarBrackets) != 0)
                yield return @"\$\(([\s\S]*?)\)";

            if ((options & SubstitutionFormatOptions.Percent) != 0)
                yield return @"\%([\s\S]*?)\%";
        }
    }
}
