using System;
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
    }
}