using System;
using Microsoft.Extensions.Configuration;

namespace Extensions.Configuration.Sources.Object
{
    internal class ObjectConfigurationSource<T> : IConfigurationSource
    {
        private readonly IConfigurationProvider provider;

        public ObjectConfigurationSource(T source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var treeDict = DictionaryProvider.GetDictionary(source);
            var flatDict = treeDict.Fold(x => x.ToString());
            this.provider = ObjectConfigurationProvider.From(flatDict);
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder) => provider;
    }
}
