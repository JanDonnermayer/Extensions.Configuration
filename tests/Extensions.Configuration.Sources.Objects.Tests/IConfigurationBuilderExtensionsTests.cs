using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace Extensions.Configuration.Sources.Objects.Tests
{
    [TestFixture]
    public class IConfigurationBuilderExtensionsTests
    {
        private IConfigurationBuilder builderMock;

        [SetUp]
        public void SetUp()
        {
            builderMock = Mock.Of<IConfigurationBuilder>();
        }

        [Test]
        public void Test_AddObject_DoesNotThrow()
        {
            // Act & Assert
            Assert.DoesNotThrow(
                () => builderMock.AddObject(("value1", "value2"))
            );
        }

        [Test]
        public void Test_AddEntry_DoesNotThrow()
        {
            // Act & Assert
            Assert.DoesNotThrow(
                () => builderMock.AddEntry("key", "value")
            );
        }

        [Test]
        public void Test_AddEntries_DoesNotThrow()
        {
            // Act & Assert
            Assert.DoesNotThrow(
                () => builderMock.AddEntries(
                    ("key1", "value1"),
                    ("key2", "value2")
                )
            );
        }
    }
}