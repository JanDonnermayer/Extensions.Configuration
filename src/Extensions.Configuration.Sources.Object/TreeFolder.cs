using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Extensions.Configuration.Sources.Object
{
    internal static class TreeFolder
    {
        public static IEnumerable<KeyValuePair<ImmutableArray<TKey>, TValue>> Fold<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, dynamic>> source,
            Func<dynamic, TValue> leafConverter
        )
        {
            IEnumerable<KeyValuePair<ImmutableArray<TKey>, TValue>> FoldInternal(ImmutableArray<TKey> path, dynamic value) =>
                value switch
                {
                    IEnumerable<KeyValuePair<TKey, dynamic>> seq =>
                        seq.SelectMany(kvp => FoldInternal(path.Add(kvp.Key), kvp.Value)),
                    var val => new[] {
                        new KeyValuePair<ImmutableArray<TKey>, TValue>(
                            key: path,
                            value: leafConverter(value)
                        )
                    }
                };

            return FoldInternal(ImmutableArray<TKey>.Empty, source);
        }
    }
}
