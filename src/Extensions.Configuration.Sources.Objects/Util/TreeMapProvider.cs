using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Extensions.Configuration.Sources.Objects
{
    internal static class TreeMapProvider
    {
        private const string CIRCULAR_OBJECT_GRAPH_DETECTED_MESSAGE =
            "Circular object-graph detected!";

        private const string OBJECT_COUNT_THRESHOLD_REACHED_MESSAGE =
            "Maximum object-count-threshold reached!" +
            "This can be a sign of a recursively generated object-graph.";

        private const int OBJECT_COUNT_THRESHOLD = 10000;

        /// <summary>
        /// Converts objects to dictionaries recursively,
        /// mapping property-names to keys and property-values to values.
        /// </summary>
        public static IReadOnlyDictionary<string, object> GetTreeMap<T>(T source)
        {
            var visited = new HashSet<object>();

            K Visit<K>(K node)
            {
                if (node is null)
                    throw new ArgumentNullException(nameof(node));

                if (!visited!.Add(node))
                    throw new InvalidOperationException(CIRCULAR_OBJECT_GRAPH_DETECTED_MESSAGE);

                if (visited.Count > OBJECT_COUNT_THRESHOLD)
                    throw new InvalidOperationException(OBJECT_COUNT_THRESHOLD_REACHED_MESSAGE);

                return node;
            }

            static bool IsUseable(PropertyInfo pi) =>
                pi.CanRead && pi.GetIndexParameters().Length == 0;

            IReadOnlyDictionary<string, object> GetDictionaryInternal<V>(V source) =>
                Visit(source)!
                    .GetType()
                    .GetProperties()
                    .Where(IsUseable)
                    .ToImmutableDictionary(
                        p => p.Name,
                        p => GetDictionaryOrValue(
                            p.GetValue(source)
                        )
                    );

            object GetDictionaryOrValue(object source) =>
                source switch
                {
                    null => throw new ArgumentNullException(nameof(source)),
                    var o when o.GetType().IsValueType => source,
                    string _ => source,
                    _ => GetDictionaryInternal(source)
                };

            return GetDictionaryInternal(source);
        }
    }
}
