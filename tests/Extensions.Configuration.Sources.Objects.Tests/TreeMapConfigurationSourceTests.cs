using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;

namespace Extensions.Configuration.Sources.Objects.Tests
{
    [TestFixture]
    public class TreeMapConfigurationSourceTests
    {
        private IConfigurationBuilder builderMock;

        [SetUp]
        public void SetUp()
        {
            this.builderMock = Mock.Of<IConfigurationBuilder>();
        }

        [Test]
        public void Test_Build_ReturnsConfigurationProvider()
        {
            // Arrange
            var source = new Dictionary<string, object>()
            {
                { "K1",  "V1" }
            };

            // Act 
            var result = TreeMapConfigurationSource
                .From(source)
                .Build(builderMock);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}