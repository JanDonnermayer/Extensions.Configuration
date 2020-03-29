using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Configuration;

namespace Extensions.Configuration.Sources.Object
{
    internal sealed partial class ObjectConfigurationProvider : IConfigurationProvider
    {
        private ImmutableDictionary<string, string> mut_dict;

        public ObjectConfigurationProvider(IEnumerable<KeyValuePair<IEnumerable<string>, string>> source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            mut_dict = source.ToImmutableDictionary(
                item => item.Key.Aggregate((x, y) => x + Constants.KEY_DELIMITER + y),
                item => item.Value
            );
        }

        public static IConfigurationProvider From(IEnumerable<KeyValuePair<IEnumerable<string>, string>> source) =>
            new ObjectConfigurationProvider(source);

        #region IConfigurationProvider

        public IChangeToken GetReloadToken() => new EmptyChangeToken();

        public void Load() { }

        public void Set(string key, string value) =>
            ImmutableInterlocked.Update(ref mut_dict, dict => dict.SetItem(key, value));

        public bool TryGet(string key, out string value) =>
            mut_dict.TryGetValue(key, out value);

        #endregion
    }
}
