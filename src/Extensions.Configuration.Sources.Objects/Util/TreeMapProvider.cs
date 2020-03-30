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

        public static IReadOnlyDictionary<string, object> GetTreeMap<T>(T source)
        {
            var visited = new HashSet<object>();

            K Visit<K>(K node)
            {
                if (node is null)
                    throw new ArgumentNullException(nameof(node));

                if (!visited.Add(node))
                    throw new InvalidOperationException(CIRCULAR_OBJECT_GRAPH_DETECTED_MESSAGE);

                if (visited.Count > OBJECT_COUNT_THRESHOLD)
                    throw new InvalidOperationException(OBJECT_COUNT_THRESHOLD_REACHED_MESSAGE);

                return node;
            }

            IReadOnlyDictionary<string, object> GetDictionaryInternal<V>(V source) =>
                Visit(source)!
                    .GetType()
                    .GetProperties()
                    .Where(p => p.CanRead)
                    .ToImmutableDictionary(
                        p => p.Name,
                        p => GetDictionaryOrValue(
                            p.GetValue(source)
                        )
                    );

            static object GetDictionaryOrValue(object source) =>
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
