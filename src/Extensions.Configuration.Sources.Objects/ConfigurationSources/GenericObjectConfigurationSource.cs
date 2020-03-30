using System;
using Microsoft.Extensions.Configuration;

namespace Extensions.Configuration.Sources.Objects
{
    internal sealed class GenericObjectConfigurationSource<T> : IConfigurationSource
    {
        private readonly IConfigurationProvider provider;

        public GenericObjectConfigurationSource(T source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var treeDict = DictionaryProvider.GetDictionary(source);
            var flatDict = treeDict.Fold(x => x.ToString());
            this.provider = ObjectConfigurationProvider.From(flatDict);
        }

        #region  IConfigurationSource

        public IConfigurationProvider Build(IConfigurationBuilder builder) => provider;

        #endregion
    }
}