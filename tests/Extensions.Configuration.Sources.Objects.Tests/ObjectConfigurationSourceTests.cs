using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace Extensions.Configuration.Sources.Objects.Tests
{
    [TestFixture]
    public class ObjectConfigurationSourceTests
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
            var source = new Box<string>("VALUE");

            // Act 
            var result = ObjectConfigurationSource
                .Of(source)
                .Build(builderMock);

            // Assert
            Assert.IsNotNull(result);
        }

        private class Box<T> { public T Value { get; } public Box(T value) => Value = value; }
    }
}