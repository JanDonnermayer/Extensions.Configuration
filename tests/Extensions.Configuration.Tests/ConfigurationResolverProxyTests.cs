using System;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace Extensions.Configuration.Tests
{
    public class ConfigurationResolverProxyTests
    {
        private IConfiguration configurationMock;

        private Func<IConfiguration, string, string> valueProviderMock;

        private IConfiguration proxy;

        [SetUp]
        public void Setup()
        {
            this.configurationMock = Mock.Of<IConfiguration>();
            this.valueProviderMock = Mock.Of<Func<IConfiguration, string, string>>();
            this.proxy = new ConfigurationResolverProxy(configurationMock, valueProviderMock);
        }

        [Test]
        public void Test_Indexer_DelegateInvokedOnSpecifiedInternalConfig()
        {
            // Arrange
            Mock.Get(configurationMock)
                .SetReturnsDefault(string.Empty);

            Mock.Get(valueProviderMock)
                .SetReturnsDefault(string.Empty);

            // Act
            _ = proxy[string.Empty];

            // Assert
            Mock.Get(valueProviderMock)
                .Verify(
                    expression: vp => vp.Invoke(
                        It.Is<IConfiguration>(cfg => cfg == configurationMock),
                        It.IsAny<string>()
                    ),
                    times: Times.Once
                );
        }
    }
}