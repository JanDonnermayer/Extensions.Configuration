// The method GetChildKeys is copied with modifications:
// Source: https://github.com/dotnet/extensions/blob/master/src/Configuration/Config/src/ConfigurationProvider.cs
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
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Extensions.Configuration.Sources.Objects
{
    internal partial class MapConfigurationProvider
    {
        #region IConfigurationProvider

        public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
        {
            var prefix = parentPath == null ? string.Empty : parentPath + ConfigurationPath.KeyDelimiter;

            return mut_dict
                .Where(kv => kv.Key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                .Select(kv => Segment(kv.Key, prefix.Length))
                .Concat(earlierKeys)
                .OrderBy(k => k, ConfigurationKeyComparer.Instance);

            static string Segment(string key, int prefixLength)
            {
                var indexOf = key.IndexOf(ConfigurationPath.KeyDelimiter, prefixLength, StringComparison.OrdinalIgnoreCase);
                return indexOf < 0 ? key.Substring(prefixLength) : key.Substring(prefixLength, indexOf - prefixLength);
            }
        }
        #endregion
    }
}
