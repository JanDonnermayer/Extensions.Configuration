using System;
using System.Collections.Immutable;
using System.Linq;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// The syntax to use for resolvers.
    /// </summary>
    [Flags]
    public enum SubstitutionSyntaxOptions
    {
        /// <summary>
        /// No substitutions are recognized.
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
        DollarBrackets = 1 << 2
    }
}
