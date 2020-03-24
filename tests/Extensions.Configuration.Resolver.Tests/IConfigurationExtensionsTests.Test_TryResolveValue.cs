using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace Extensions.Configuration.Resolver.Tests
{
    public partial class IConfigurationExtensionsTests
    {
        public class Test_TryResolveValue
        {
            private IConfiguration configurationMock;

            [SetUp]
            public void Setup()
            {
                this.configurationMock = Mock.Of<IConfiguration>();
            }

            [Test]
            public void ProvidableValue_ReturnsTrue()
            {
                // Arrange
                const string KEY = "key";
                const string VALUE = "val";

                Mock.Get(configurationMock)
                    .SetupGet(cfg => cfg[It.Is<string>(k => k == KEY)])
                    .Returns(VALUE);

                // Act
                var result = configurationMock
                    .TryResolveValue(KEY, out var _);

                // Assert
                Assert.IsTrue(result);
            }

            [Test]
            public void ProvidableValue_YieldsValue()
            {
                // Arrange
                const string KEY = "key";
                const string VALUE = "val";

                Mock.Get(configurationMock)
                    .SetupGet(cfg => cfg[It.Is<string>(k => k == KEY)])
                    .Returns(VALUE);

                // Act
                configurationMock
                    .TryResolveValue(KEY, out var actualValue);

                // Assert
                Assert.AreEqual(VALUE, actualValue);
            }

            [Test]
            public void UnprovidableValue_ReturnsFalse()
            {
                // Arrange
                const string KEY = "key";

                // Act
                var result = configurationMock
                    .TryResolveValue(KEY, out var _);

                // Assert
                Assert.IsFalse(result);
            }

            [Test]
            public void UnprovidableValue_DoesNotThrow()
            {
                // Arrange
                const string KEY = "key";

                // Act & Assert
                Assert.DoesNotThrow(
                    () => configurationMock.TryResolveValue(KEY, out var _)
                );
            }
        }
    }
}