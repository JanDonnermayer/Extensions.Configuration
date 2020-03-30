using System.Collections.Generic;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;

namespace Extensions.Configuration.Sources.Objects.Tests
{
    [TestFixture]
    public class MapConfigurationProviderTests
    {
        [Test]
        public void Test_GetValue()
        {
            // Arrange
            var source = new Dictionary<string, string>()
            {
                { "K1",  "V1" }
            };

            // Act
            var result = MapConfigurationProvider
                .Of(source)
                .TryGetValue("K1");

            // Assert
            Assert.AreEqual((true, "V1"), result);
        }
    }
}