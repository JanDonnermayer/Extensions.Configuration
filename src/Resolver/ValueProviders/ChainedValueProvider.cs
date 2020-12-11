using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Extensions.Configuration.Resolver
{
    internal class ChainedValueProvider : IValueProvider
    {
        private readonly IEnumerable<IValueProvider> valueProviders;

        public ChainedValueProvider(IEnumerable<IValueProvider> valueProviders)
        {
            this.valueProviders = valueProviders.ToImmutableList();
        }

        public string GetValue(string key) =>
            TryGetValue(key) switch
            {
                (true, var val) => val!,
                _ => throw new KeyNotFoundException($"No such key: '{key}'")
            };

        public (bool success, string? value) TryGetValue(string key) =>
            valueProviders
                .Select(p => p.TryGetValue(key))
                .FirstOrDefault(v => v.success);
    }
}
