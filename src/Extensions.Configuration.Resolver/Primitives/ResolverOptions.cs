using System;


namespace Extensions.Configuration.Resolver
{
    /// <summary>
    /// The key-substitution syntax to use for resolvers.
    /// </summary>
    [Flags]
    public enum ResolverOptions
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
