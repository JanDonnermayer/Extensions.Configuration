using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Configuration
{
    internal static class SubstitutionSyntaxExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="syntaxOptions"/> to their corresponding 
        /// Regular Expressions.
        /// </summary>
        public static IEnumerable<string> ToRegexPatterns(this SubstitutionSyntaxOptions syntaxOptions)
        {
            if ((syntaxOptions & SubstitutionSyntaxOptions.CurlyBracketsDollarEnv) != 0)
                yield return @"\{\$env:([\s\S]*?)\}";

            if ((syntaxOptions & SubstitutionSyntaxOptions.DollarCurlyBrackets) != 0)
                yield return @"\$\{([\s\S]*?)\}";

            if ((syntaxOptions & SubstitutionSyntaxOptions.DollarBrackets) != 0)
                yield return @"\$\(([\s\S]*?)\)";
        }
    }
}
