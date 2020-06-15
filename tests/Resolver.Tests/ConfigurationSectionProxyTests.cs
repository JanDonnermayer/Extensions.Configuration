using System;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace Extensions.Configuration.Resolver.Tests
{
    public class ConfigurationSectionProxyTests
    {
        private IConfigurationSection configurationSectionMock;

        private Func<IConfiguration, string, string> valueProviderMock;

        private ConfigurationSectionProxy proxy;


        [SetUp]
        public void Setup()
        {
            this.configurationSectionMock = Mock.Of<IConfigurationSection>();
            this.valueProviderMock = Mock.Of<Func<IConfiguration, string, string>>();
            this.proxy = new ConfigurationSectionProxy(configurationSectionMock, valueProviderMock);
        }

        [Test]
        public void Test_Indexer_DelegateInvokedOnSpecifiedInternalConfig()
        {
            // Arrange
            Mock.Get(configurationSectionMock)
                .SetReturnsDefault(string.Empty);

            Mock.Get(valueProviderMock)
                .SetReturnsDefault(string.Empty);

            // Act
            _ = proxy[string.Empty];

            // Assert
            Mock.Get(valueProviderMock)
                .Verify(
                    expression: vp => vp.Invoke(
                        It.Is<IConfiguration>(cfg => cfg == configurationSectionMock),
                        It.IsAny<string>()
                    ),
                    times: Times.Once
                );
        }

        [TestCase("val")]
        [TestCase("")]
        [TestCase(null)]
        public void Test_Indexer_ReturnsDelegateReturnValue(string delegateReturnValue)
        {
            // Arrange
            Mock.Get(configurationSectionMock)
                .SetReturnsDefault<string>(null);

            Mock.Get(valueProviderMock)
                .SetReturnsDefault(delegateReturnValue);

            // Act
            var actualReturnValue = proxy[string.Empty];

            // Assert
            Assert.AreEqual(delegateReturnValue, actualReturnValue);
        }

        [TestCase("val")]
        [TestCase("")]
        [TestCase(null)]
        public void Test_GetSection_ReturnsNewConfigurationSectionProxy(string sectionKey)
        {
            // Arrange
            Mock.Get(configurationSectionMock)
                .SetReturnsDefault(configurationSectionMock);

            // Act
            var section = proxy.GetSection(sectionKey);

            // Assert
            Assert.IsInstanceOf<ConfigurationSectionProxy>(section);
            Assert.AreNotSame(section, configurationSectionMock);
        }
    }
}
