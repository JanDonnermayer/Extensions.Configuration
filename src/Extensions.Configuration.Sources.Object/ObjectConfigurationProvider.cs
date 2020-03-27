using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Extensions.Configuration.Sources.Object
{
    internal class DicitionaryConfigurationProvider : ConfigurationProvider
    {
        public DicitionaryConfigurationProvider(IEnumerable<KeyValuePair<string, object>> source)
        {
            throw new NotImplementedException();
        }
    }

    internal class DictionaryConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            throw new NotImplementedException();
        }
    }

}
