using System;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace Extensions.Configuration.Tests
{
    public class IConfigurationExtensionsTests
    {
        private IConfiguration configurationMock;

        [SetUp]
        public void Setup()
        {
            this.configurationMock = Mock.Of<IConfiguration>();
        }

        [Test]
        public void Test_ResolveValue_ZeroResolveSteps_ResolvesCorrectly()
        {
            // Arrange
            const string KEY_1 = "key1";
            const string VALUE_1 = "val1";

            Mock.Get(configurationMock)
                .SetupGet(cfg => cfg[It.Is<string>(k => k == KEY_1)])
                .Returns(VALUE_1);

            // Act
            var actualValue = configurationMock.ResolveValue(KEY_1);

            // Assert
            Assert.AreEqual(VALUE_1, actualValue);
        }

        
        [Test]
        public void Test_ResolveValue_ZeroResolveSteps_KeyEmptyString_ResolvesCorrectly()
        {
            // Arrange
            const string KEY_1 = "";
            const string VALUE_1 = "val1";

            Mock.Get(configurationMock)
                .SetupGet(cfg => cfg[It.Is<string>(k => k == KEY_1)])
                .Returns(VALUE_1);

            // Act
            var actualValue = configurationMock.ResolveValue(KEY_1);

            // Assert
            Assert.AreEqual(VALUE_1, actualValue);
        }

        [Test]
        public void Test_ResolveValue_OneResolveStep_ResolvesCorrectly()
        {
            // Arrange
            const string KEY_1 = "key1";
            const string KEY_2 = "key2";
            const string VALUE_1 = "{$env:" + KEY_2 + "}";
            const string VALUE_2 = "val2";

            Mock.Get(configurationMock)
                .SetupGet(cfg => cfg[It.Is<string>(k => k == KEY_1)])
                .Returns(VALUE_1);

            Mock.Get(configurationMock)
                .SetupGet(cfg => cfg[It.Is<string>(k => k == KEY_2)])
                .Returns(VALUE_2);

            // Act
            var actualValue = configurationMock.ResolveValue(KEY_1);

            // Assert
            Assert.AreEqual(VALUE_2, actualValue);
        }


        [Test]
        public void Test_ResolveValue_TwoResolveSteps_ResolvesCorrectly()
        {
            // Arrange
            const string KEY_1 = "key1";
            const string KEY_2 = "key2";
            const string KEY_3 = "key3";
            const string VALUE_1 = "{$env:" + KEY_2 + "}";
            const string VALUE_2 = "{$env:" + KEY_3 + "}";
            const string VALUE_3 = "val2";

            Mock.Get(configurationMock)
                .SetupGet(cfg => cfg[It.Is<string>(k => k == KEY_1)])
                .Returns(VALUE_1);

            Mock.Get(configurationMock)
                .SetupGet(cfg => cfg[It.Is<string>(k => k == KEY_2)])
                .Returns(VALUE_2);

            Mock.Get(configurationMock)
                .SetupGet(cfg => cfg[It.Is<string>(k => k == KEY_3)])
                .Returns(VALUE_3);

            // Act
            var actualValue = configurationMock.ResolveValue(KEY_1);

            // Assert
            Assert.AreEqual(VALUE_3, actualValue);
        }


        [Test]
        public void Test_ResolveValue_OneResolveStep_Loop_ThrowsInvalidOperationException()
        {
            // Arrange
            const string KEY_1 = "key1";
            const string VALUE_1 = "{$env:" + KEY_1 + "}";

            Mock.Get(configurationMock)
                .SetupGet(cfg => cfg[It.Is<string>(k => k == KEY_1)])
                .Returns(VALUE_1);

            // AssertThrows
            Assert.Throws<InvalidOperationException>(
                () => configurationMock.ResolveValue(KEY_1)
            );
        }

        [Test]
        public void Test_ResolveValue_KeyNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => configurationMock.ResolveValue(null)
            );
        }

    }
}