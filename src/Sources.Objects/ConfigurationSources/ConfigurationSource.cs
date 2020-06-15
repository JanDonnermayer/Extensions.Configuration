using Microsoft.Extensions.Configuration;

namespace Extensions.Configuration.Sources.Objects
{
    internal sealed class ConfigurationSource : IConfigurationSource
    {
        private readonly IConfigurationProvider provider;

        public ConfigurationSource(IConfigurationProvider provider) =>
            this.provider = provider ?? throw new System.ArgumentNullException(nameof(provider));

        #region  IConfigurationSource

        public IConfigurationProvider Build(IConfigurationBuilder builder) => provider;

        #endregion

        public static IConfigurationSource FromProvider(IConfigurationProvider provider) =>
            new ConfigurationSource(provider);
    }
}
