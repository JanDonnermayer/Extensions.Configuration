using System.Collections.Generic;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;

namespace Extensions.Configuration.Sources.Object.Tests
{
    [TestFixture]
    public class ObjectConfigurationProviderTests
    {
        private const string KEY_DELIMITER = ":";

        [Test]
        public void Test_OneNestingLevel_ContainsValue()
        {
            // Arrange
            var source = new Dictionary<IEnumerable<string>, string>()
            {
                { new [] { "K1", "K11"}, "V11" }
            };

            // Act
            var result = ObjectConfigurationProvider
                .From(source)
                .TryGetValue("K1" + KEY_DELIMITER + "K11");

            // Assert
            Assert.AreEqual((true, "V11"), result);
        }

        [Test]
        public void Test_ZeroNestingLevels_ContainsValue()
        {
            // Arrange
            var source = new Dictionary<IEnumerable<string>, string>()
            {
                { new [] { "K1" }, "V1" }
            };

            // Act
            var result = ObjectConfigurationProvider
                .From(source)
                .TryGetValue("K1");

            // Assert
            Assert.AreEqual((true, "V1"), result);
        }
    }
}