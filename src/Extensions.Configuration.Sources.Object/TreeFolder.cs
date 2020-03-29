using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Extensions.Configuration.Sources.Object
{
    internal static class TreeFolder
    {
        /// <summary>
        /// Converts a sequence of trees of <see cref="KeyValuePair"/> into a flat sequence of
        /// <see cref="KeyValuePair"/> aggregating keys into sequences.
        /// </summary>
        /// <Example>
        /// [ { K1, { K11, V1 } }, { K2, V2 } ] => [ { [ K1, K11 ], V1 }, { [ K2 ], V2 } ]
        /// </Example>
        public static IEnumerable<KeyValuePair<IEnumerable<TKey>, TValue>> Fold<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, object>> source,
            Func<object, TValue> leafConverter
        )
        {
            IEnumerable<KeyValuePair<IEnumerable<TKey>, TValue>> FoldInternal(ImmutableList<TKey> path, object value) =>
                value switch
                {
                    IEnumerable<KeyValuePair<TKey, dynamic>> seq =>
                        seq.SelectMany(
                            kvp => FoldInternal(path.Add(kvp.Key), kvp.Value)
                        ),
                    var val =>
                        new[] {
                            new KeyValuePair<IEnumerable<TKey>, TValue>(
                                key: path,
                                value: leafConverter(value)
                            )
                        }
                };

            return FoldInternal(ImmutableList<TKey>.Empty, source);
        }
    }
}
