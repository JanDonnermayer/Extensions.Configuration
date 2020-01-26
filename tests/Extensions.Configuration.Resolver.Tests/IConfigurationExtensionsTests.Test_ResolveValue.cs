using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace Extensions.Configuration.Tests
{
    public partial class IConfigurationExtensionsTests
    {
        public class Test_ResolveValue
        {
            #region TestCaseSources

            private const ResolverOptions OPTIONS_1 =
                ResolverOptions.CurlyBracketsDollarEnv;
            private const string OPTIONS_1_PREFIX = "{$env:";
            private const string OPTIONS_1_SUFFIX = "}";

            private const ResolverOptions OPTIONS_2 =
                ResolverOptions.DollarCurlyBrackets;
            private const string OPTIONS_2_PREFIX = "${";
            private const string OPTIONS_2_SUFFIX = "}";

            private const ResolverOptions OPTIONS_3 =
                ResolverOptions.DollarBrackets;
            private const string OPTIONS_3_PREFIX = "$(";
            private const string OPTIONS_3_SUFFIX = ")";

            private const ResolverOptions OPTIONS_4 =
                ResolverOptions.Percent;
            private const string OPTIONS_4_PREFIX = "%";
            private const string OPTIONS_4_SUFFIX = "%";

            #endregion

            private IConfiguration configurationMock;

            [SetUp]
            public void Setup()
            {
                this.configurationMock = Mock.Of<IConfiguration>();
            }

            [TestCase(OPTIONS_1)]
            [TestCase(OPTIONS_2)]
            [TestCase(OPTIONS_3)]
            [TestCase(OPTIONS_4)]
            public void ZeroResolveSteps_OneKeyPerStep_ResolvesCorrectly(
                ResolverOptions options
            )
            {
                // Arrange
                const string KEY_1 = "key1";
                const string VALUE_1 = "val1";

                Mock.Get(configurationMock)
                    .SetupGet(cfg => cfg[It.Is<string>(k => k == KEY_1)])
                    .Returns(VALUE_1);

                // Act
                var actualValue = configurationMock.ResolveValue(KEY_1, options);

                // Assert
                Assert.AreEqual(VALUE_1, actualValue);
            }

            [TestCase(OPTIONS_1)]
            [TestCase(OPTIONS_2)]
            [TestCase(OPTIONS_3)]
            [TestCase(OPTIONS_4)]
            public void ZeroResolveSteps_OneKeyPerStep_KeyEmptyString_ResolvesCorrectly(
                ResolverOptions options
            )
            {
                // Arrange
                const string KEY_1 = "";
                const string VALUE_1 = "val1";

                Mock.Get(configurationMock)
                    .SetupGet(cfg => cfg[It.Is<string>(k => k == KEY_1)])
                    .Returns(VALUE_1);

                // Act
                var actualValue = configurationMock.ResolveValue(KEY_1, options);

                // Assert
                Assert.AreEqual(VALUE_1, actualValue);
            }

            [TestCase(OPTIONS_1, OPTIONS_1_PREFIX, OPTIONS_1_SUFFIX)]
            [TestCase(OPTIONS_2, OPTIONS_2_PREFIX, OPTIONS_2_SUFFIX)]
            [TestCase(OPTIONS_3, OPTIONS_3_PREFIX, OPTIONS_3_SUFFIX)]
            [TestCase(OPTIONS_4, OPTIONS_4_PREFIX, OPTIONS_4_SUFFIX)]
            public void OneResolveStep_OneKeyPerStep_ResolvesCorrectly(
                ResolverOptions options, string prefix, string suffix
            )
            {
                // Arrange
                const string KEY_1 = "key1";
                const string KEY_2 = "key2";
                string VALUE_1 = prefix + KEY_2 + suffix;
                const string VALUE_2 = "val2";

                Mock.Get(configurationMock)
                    .SetupGet(cfg => cfg[It.Is<string>(k => k == KEY_1)])
                    .Returns(VALUE_1);

                Mock.Get(configurationMock)
                    .SetupGet(cfg => cfg[It.Is<string>(k => k == KEY_2)])
                    .Returns(VALUE_2);

                // Act
                var actualValue = configurationMock.ResolveValue(KEY_1, options);

                // Assert
                Assert.AreEqual(VALUE_2, actualValue);
            }

            [TestCase(OPTIONS_1, OPTIONS_1_PREFIX, OPTIONS_1_SUFFIX)]
            [TestCase(OPTIONS_2, OPTIONS_2_PREFIX, OPTIONS_2_SUFFIX)]
            [TestCase(OPTIONS_3, OPTIONS_3_PREFIX, OPTIONS_3_SUFFIX)]
            [TestCase(OPTIONS_4, OPTIONS_4_PREFIX, OPTIONS_4_SUFFIX)]
            public void OneResolveStep_TwoKeysPerStep_ResolvesCorrectly(
                ResolverOptions options, string prefix, string suffix
            )
            {
                // Arrange
                const string KEY_1 = "key1";
                const string KEY_2 = "key2";
                string VALUE_1 = prefix + KEY_2 + suffix + prefix + KEY_2 + suffix;
                const string VALUE_2 = "val2";

                Mock.Get(configurationMock)
                    .SetupGet(cfg => cfg[It.Is<string>(k => k == KEY_1)])
                    .Returns(VALUE_1);

                Mock.Get(configurationMock)
                    .SetupGet(cfg => cfg[It.Is<string>(k => k == KEY_2)])
                    .Returns(VALUE_2);

                // Act
                var actualValue = configurationMock.ResolveValue(KEY_1, options);

                // Assert
                Assert.AreEqual(VALUE_2 + VALUE_2, actualValue);
            }

            [TestCase(OPTIONS_1, OPTIONS_1_PREFIX, OPTIONS_1_SUFFIX)]
            [TestCase(OPTIONS_2, OPTIONS_2_PREFIX, OPTIONS_2_SUFFIX)]
            [TestCase(OPTIONS_3, OPTIONS_3_PREFIX, OPTIONS_3_SUFFIX)]
            [TestCase(OPTIONS_4, OPTIONS_4_PREFIX, OPTIONS_4_SUFFIX)]
            public void TwoResolveSteps_OneKeyPerStep_ResolvesCorrectly(
                ResolverOptions options, string prefix, string suffix
            )
            {
                // Arrange
                const string KEY_1 = "key1";
                const string KEY_2 = "key2";
                const string KEY_3 = "key3";
                string VALUE_1 = prefix + KEY_2 + suffix;
                string VALUE_2 = prefix + KEY_3 + suffix;
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
                var actualValue = configurationMock.ResolveValue(KEY_1, options);

                // Assert
                Assert.AreEqual(VALUE_3, actualValue);
            }

            [TestCase(OPTIONS_1, OPTIONS_1_PREFIX, OPTIONS_1_SUFFIX)]
            [TestCase(OPTIONS_2, OPTIONS_2_PREFIX, OPTIONS_2_SUFFIX)]
            [TestCase(OPTIONS_3, OPTIONS_3_PREFIX, OPTIONS_3_SUFFIX)]
            [TestCase(OPTIONS_4, OPTIONS_4_PREFIX, OPTIONS_4_SUFFIX)]
            public void InfiniteSteps_OneKeyPerStep_ThrowsInvalidOperationException(
                ResolverOptions options, string prefix, string suffix
            )
            {
                // Arrange
                const string KEY_1 = "key1";
                string VALUE_1 = prefix + KEY_1 + suffix;

                Mock.Get(configurationMock)
                    .SetupGet(cfg => cfg[It.Is<string>(k => k == KEY_1)])
                    .Returns(VALUE_1);

                // AssertThrows
                Assert.Throws<InvalidOperationException>(
                    () => configurationMock.ResolveValue(KEY_1, options)
                );
            }

            [TestCase(OPTIONS_1, OPTIONS_1_PREFIX, OPTIONS_1_SUFFIX)]
            [TestCase(OPTIONS_2, OPTIONS_2_PREFIX, OPTIONS_2_SUFFIX)]
            [TestCase(OPTIONS_3, OPTIONS_3_PREFIX, OPTIONS_3_SUFFIX)]
            [TestCase(OPTIONS_4, OPTIONS_4_PREFIX, OPTIONS_4_SUFFIX)]
            public void InfiniteSteps_TwoKeysPerStep_ThrowsInvalidOperationException(
                ResolverOptions options, string prefix, string suffix
            )
            {
                // Arrange
                const string KEY_1 = "key1";
                string VALUE_1 = prefix + KEY_1 + suffix + prefix + KEY_1 + suffix;

                Mock.Get(configurationMock)
                    .SetupGet(cfg => cfg[It.Is<string>(k => k == KEY_1)])
                    .Returns(VALUE_1);

                // AssertThrows
                Assert.Throws<InvalidOperationException>(
                    () => configurationMock.ResolveValue(KEY_1, options)
                );
            }

            [TestCase(OPTIONS_1, OPTIONS_1_PREFIX, OPTIONS_1_SUFFIX)]
            [TestCase(OPTIONS_2, OPTIONS_2_PREFIX, OPTIONS_2_SUFFIX)]
            [TestCase(OPTIONS_3, OPTIONS_3_PREFIX, OPTIONS_3_SUFFIX)]
            [TestCase(OPTIONS_4, OPTIONS_4_PREFIX, OPTIONS_4_SUFFIX)]
            public void KeyNotExists_ThrowsKeyNotFoundException(
                ResolverOptions options, string prefix, string suffix
            )
            {
                Assert.Throws<KeyNotFoundException>(
                    () => configurationMock.ResolveValue(prefix + suffix, options)
                );
            }

            [Test]
            public void KeyNull_ThrowsArgumentNullException()
            {
                Assert.Throws<ArgumentNullException>(
                    () => configurationMock.ResolveValue(null)
               );
            }
        }
    }
}