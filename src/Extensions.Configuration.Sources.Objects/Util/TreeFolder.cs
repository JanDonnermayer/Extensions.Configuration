using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Extensions.Configuration.Sources.Objects
{
    internal static class TreeFolder
    {
        /// <summary>
        /// Converts a sequence of trees of KeyValuePairs into a flat sequence of
        /// KeyValuePairs, aggregating keys into sequences.
        /// </summary>
        /// <param name="source">A sequence of trees of KeyValuePairs</param>
        /// <param name="valueConverter">A function that converts the values of KeyValuePairs at leafs.</param>
        /// <typeparam name="TKey">The key-type of the KeyValuePairs.</typeparam>
        /// <typeparam name="TValue">The type into which the values of KeyValuePairs at leafs are converted.</typeparam>
        /// <Returns>A flat sequence of KeyValuePairs, where the keys represent the path of keys in the source.</Returns>
        /// <Example>
        /// [ { K1, { K11, V11 } }, { K2, V2 } ] => [ { [ K1, K11 ], V11 }, { [ K2 ], V2 } ]
        /// </Example>
        public static IEnumerable<KeyValuePair<IEnumerable<TKey>, TValue>> Fold<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, object>> source,
            Func<object, TValue> valueConverter
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (valueConverter is null)
                throw new ArgumentNullException(nameof(valueConverter));

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
                                value: valueConverter(value)
                            )
                        }
                };

            return FoldInternal(ImmutableList<TKey>.Empty, source);
        }
    }
}
