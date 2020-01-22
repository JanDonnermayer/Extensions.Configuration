using System;
using System.Collections.Immutable;
using System.Linq;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// The syntax to use for resolvers.
    /// </summary>
    [Flags]
    public enum SubstitutionFormatOptions
    {
        /// <summary>
        /// No format.
        /// </summary>
        None = 0,

        /// <summary>
        /// {$env:KEY}
        /// </summary>
        CurlyBracketsDollarEnv = 1,

        /// <summary>
        /// ${KEY}
        /// </summary>
        DollarCurlyBrackets = 1 << 1,

        /// <summary>
        /// $(KEY)
        /// </summary>
        DollarBrackets = 1 << 2,

        /// <summary>
        /// %KEY%
        /// </summary>
        Percent = 1 << 3,

        /// <summary>
        /// All other formats.
        /// </summary>
        All = CurlyBracketsDollarEnv
            | DollarCurlyBrackets
            | DollarBrackets
            | Percent
    }
}
