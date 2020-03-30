using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Extensions.Configuration.Sources.Objects
{
    internal sealed class MapConfigurationSource
    {
        private readonly IConfigurationProvider provider;

        public MapConfigurationSource(IEnumerable<KeyValuePair<IEnumerable<string>, string>> source)
        {
            if (source is null)
                throw new System.ArgumentNullException(nameof(source));

            this.provider = MapConfigurationProvider.From(source);
        }
    }
}
