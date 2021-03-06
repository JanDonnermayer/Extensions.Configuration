using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Configuration;

namespace Extensions.Configuration.Sources.Objects
{
    internal sealed partial class MapConfigurationProvider : IConfigurationProvider
    {
        private ImmutableDictionary<string, string> mut_dict;

        public MapConfigurationProvider(IEnumerable<KeyValuePair<string, string>> source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            mut_dict = source.ToImmutableDictionary(
                item => item.Key,
                item => item.Value
            );
        }

        #region IConfigurationProvider

        public IChangeToken GetReloadToken() => new EmptyChangeToken();

        public void Load() { }

        public void Set(string key, string value) =>
            ImmutableInterlocked.Update(ref mut_dict, dict => dict.SetItem(key, value));

        public bool TryGet(string key, out string value) =>
            mut_dict.TryGetValue(key, out value);

        #endregion

        public static IConfigurationProvider FromEntries(IEnumerable<KeyValuePair<string, string>> entries) =>
            new MapConfigurationProvider(entries);

        public static IConfigurationProvider FromEntries(IEnumerable<(string, string)> entries) =>
            FromEntries(entries.Select(e => new KeyValuePair<string, string>(e.Item1, e.Item2)));
    }
}
