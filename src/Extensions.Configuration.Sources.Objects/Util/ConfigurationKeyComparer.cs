// This code is copied:
// Source: https://github.com/dotnet/extensions/blob/master/src/Configuration/Config/src/ConfigurationKeyComparer.cs
// Commit: 321a30c on 26 Feb 2020
// Reason: Avoid dependency

// Original Header:
// ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// +  Licensed to the .NET Foundation under one or more agreements.         +
// +  The .NET Foundation licenses this file to you under the MIT license.  +
// +  See the LICENSE file in the project root for more information.        +
// ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Extensions.Configuration.Sources.Objects
{
    /// <summary>
    /// IComparer implementation used to order configuration keys.
    /// </summary>
    internal class ConfigurationKeyComparer : IComparer<string>
    {
        private static readonly string[] _keyDelimiterArray = new[] { ConfigurationPath.KeyDelimiter };

        /// <summary>
        /// The default instance.
        /// </summary>
        public static ConfigurationKeyComparer Instance { get; } = new ConfigurationKeyComparer();

        /// <summary>
        /// Compares two strings.
        /// </summary>
        /// <param name="x">First string.</param>
        /// <param name="y">Second string.</param>
        /// <returns>Less than 0 if x is less than y, 0 if x is equal to y and greater than 0 if x is greater than y.</returns>
        public int Compare(string x, string y)
        {
            var xParts = x?.Split(_keyDelimiterArray, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
            var yParts = y?.Split(_keyDelimiterArray, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();

            // Compare each part until we get two parts that are not equal
            for (int i = 0; i < Math.Min(xParts.Length, yParts.Length); i++)
            {
                x = xParts[i];
                y = yParts[i];

                var value1 = 0;
                var value2 = 0;

                var xIsInt = x != null && int.TryParse(x, out value1);
                var yIsInt = y != null && int.TryParse(y, out value2);

                int result;

                if (!xIsInt && !yIsInt)
                {
                    // Both are strings
                    result = string.Compare(x, y, StringComparison.OrdinalIgnoreCase);
                }
                else if (xIsInt && yIsInt)
                {
                    // Both are int
                    result = value1 - value2;
                }
                else
                {
                    // Only one of them is int
                    result = xIsInt ? -1 : 1;
                }

                if (result != 0)
                {
                    // One of them is different
                    return result;
                }
            }

            // If we get here, the common parts are equal.
            // If they are of the same length, then they are totally identical
            return xParts.Length - yParts.Length;
        }
    }
}