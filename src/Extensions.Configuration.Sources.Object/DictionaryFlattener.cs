using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Extensions.Configuration.Sources.Object
{
    internal static class DictionaryFolder
    {
        public static IEnumerable<KeyValuePair<ImmutableArray<TKey>, TValue>> Fold<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, dynamic>> source)
        {
            static IEnumerable<KeyValuePair<ImmutableArray<TKey>, TValue>> FoldInternal(ImmutableArray<TKey> path, dynamic value) =>
                value switch
                {
                    IEnumerable<KeyValuePair<TKey, dynamic>> seq =>
                        seq.SelectMany(kvp => FoldInternal(path.Add(kvp.Key), kvp.Value)),
                    var val => new[] { new KeyValuePair<ImmutableArray<TKey>, TValue>(path, value) }
                };

            return FoldInternal(ImmutableArray<TKey>.Empty, source);
        }
    }
}
