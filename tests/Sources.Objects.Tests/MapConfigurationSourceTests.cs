using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using System;

namespace Extensions.Configuration.Sources.Objects.Tests
{
    [TestFixture]
    public class MapConfigurationSourceTests
    {
        private IConfigurationBuilder builderMock;

        [SetUp]
        public void SetUp()
        {
            this.builderMock = Mock.Of<IConfigurationBuilder>();
        }

        [Test]
        public void Test_FromDictionary_Build_ReturnsConfigurationProvider()
        {
            // Arrange
            var source = new Dictionary<string, string>()
            {
                { "K1",  "V1" }
            };

            // Act 
            var result = MapConfigurationSource
                .FromEntries(source)
                .Build(builderMock);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}