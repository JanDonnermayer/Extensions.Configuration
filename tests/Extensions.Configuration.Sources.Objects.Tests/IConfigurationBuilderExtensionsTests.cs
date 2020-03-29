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
        public void Test_AddObject_AddsConfigurationSource()
        {
            // Arrange 
            var obj = ("key", "value");

            // Act
            builderMock.AddObject(obj);

            // Assert
            Mock.Get(builderMock)
                .Verify(
                    b => b.Add(It.IsAny<IConfigurationSource>()),
                    Times.Once
                );

            Mock.Get(builderMock)
                .VerifyNoOtherCalls();
        }
    }
}