using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Extensions.Configuration.Sources.Objects.Tests
{
    [TestFixture]
    public class TreeFolderTests
    {
        [Test]
        public void Test_Fold()
        {
            // Arrange
            var V1 = new Dictionary<string, object>()
            {
                { "K11", "V11" },
                { "K12", "V12" }
            };

            var source = new Dictionary<string, object>()
            {
                { "K1", V1 },
                { "K2", "V2" }
            };

            var expected = new Dictionary<IEnumerable<string>, string>()
            {
                { new [] { "K1", "K11" }, "V11" },
                { new [] { "K1", "K12" }, "V12" },
                { new [] { "K2" }, "V2" },
            };

            // Act
            var result = source.Fold(x => x);

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}