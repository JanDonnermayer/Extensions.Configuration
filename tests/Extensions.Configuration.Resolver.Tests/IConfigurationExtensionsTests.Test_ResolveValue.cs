using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace Extensions.Configuration.Resolver.Tests
{
    public partial class IConfigurationExtensionsTests
    {
        public class Test_ResolveValue
        {
            #region TestCaseSources

            private const SubstitutionOptions OPTIONS_1 =
                SubstitutionOptions.CurlyBracketsDollarEnv;
            private const string OPTIONS_1_PREFIX = "{$env:";
            private const string OPTIONS_1_SUFFIX = "}";

            private const SubstitutionOptions OPTIONS_2 =
                SubstitutionOptions.DollarCurlyBrackets;
            private const string OPTIONS_2_PREFIX = "${";
            private const string OPTIONS_2_SUFFIX = "}";

            private const SubstitutionOptions OPTIONS_3 =
                SubstitutionOptions.DollarBrackets;
            private const string OPTIONS_3_PREFIX = "$(";
            private const string OPTIONS_3_SUFFIX = ")";

            private const SubstitutionOptions OPTIONS_4 =
                SubstitutionOptions.Percent;
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
            public void ZeroResolveSteps_OneKeyPerStep_ReturnsResolved(
                SubstitutionOptions options
            )
            {
                // Arrange
                const string KEY_1 = "key1";
                const string VALUE_1 = "val1";

                configurationMock = Mock.Of<IConfiguration>(
                    c => c[KEY_1] == VALUE_1
                );

                // Act
                var actualValue = configurationMock.ResolveValue(KEY_1, options);

                // Assert
                Assert.AreEqual(VALUE_1, actualValue);
            }

            [TestCase(OPTIONS_1)]
            [TestCase(OPTIONS_2)]
            [TestCase(OPTIONS_3)]
            [TestCase(OPTIONS_4)]
            public void ZeroResolveSteps_OneKeyPerStep_KeyEmptyString_ReturnsResolved(
                SubstitutionOptions options
            )
            {
                // Arrange
                const string KEY_1 = "";
                const string VALUE_1 = "val1";

                configurationMock = Mock.Of<IConfiguration>(
                    c => c[KEY_1] == VALUE_1
                );

                // Act
                var actualValue = configurationMock.ResolveValue(KEY_1, options);

                // Assert
                Assert.AreEqual(VALUE_1, actualValue);
            }

            [TestCase(OPTIONS_1, OPTIONS_1_PREFIX, OPTIONS_1_SUFFIX)]
            [TestCase(OPTIONS_2, OPTIONS_2_PREFIX, OPTIONS_2_SUFFIX)]
            [TestCase(OPTIONS_3, OPTIONS_3_PREFIX, OPTIONS_3_SUFFIX)]
            [TestCase(OPTIONS_4, OPTIONS_4_PREFIX, OPTIONS_4_SUFFIX)]
            public void OneResolveStep_OneKeyPerStep_ReturnsResolved(
                SubstitutionOptions options, string prefix, string suffix
            )
            {
                // Arrange
                const string KEY_1 = "key1";
                const string KEY_2 = "key2";
                string VALUE_1 = prefix + KEY_2 + suffix;
                const string VALUE_2 = "val2";

                configurationMock = Mock.Of<IConfiguration>(c =>
                    c[KEY_1] == VALUE_1 &&
                    c[KEY_2] == VALUE_2
                );

                // Act
                var actualValue = configurationMock.ResolveValue(KEY_1, options);

                // Assert
                Assert.AreEqual(VALUE_2, actualValue);
            }

            [TestCase(OPTIONS_1, OPTIONS_1_PREFIX, OPTIONS_1_SUFFIX)]
            [TestCase(OPTIONS_2, OPTIONS_2_PREFIX, OPTIONS_2_SUFFIX)]
            [TestCase(OPTIONS_3, OPTIONS_3_PREFIX, OPTIONS_3_SUFFIX)]
            [TestCase(OPTIONS_4, OPTIONS_4_PREFIX, OPTIONS_4_SUFFIX)]
            public void OneResolveStep_OneKeyPerStep_FirstStepUnresolvable_InvokesMapValueFunction(
                SubstitutionOptions options, string prefix, string suffix
            )
            {
                // Arrange
                const string KEY_1 = "key1";
                const string KEY_2 = "key2";
                string VALUE_1 = prefix + KEY_2 + suffix;

                configurationMock = Mock.Of<IConfiguration>(
                    c => c[KEY_1] == VALUE_1
                );

                var mapValueMock = Mock.Of<Func<string, string>>();

                // Act
                configurationMock.ResolveValue(KEY_1, options, mapValueMock);

                // Assert
                Mock.Get(mapValueMock)
                    .Verify(m => m(It.Is<string>(v => v == VALUE_1)));
            }

            [TestCase(OPTIONS_1, OPTIONS_1_PREFIX, OPTIONS_1_SUFFIX)]
            [TestCase(OPTIONS_2, OPTIONS_2_PREFIX, OPTIONS_2_SUFFIX)]
            [TestCase(OPTIONS_3, OPTIONS_3_PREFIX, OPTIONS_3_SUFFIX)]
            [TestCase(OPTIONS_4, OPTIONS_4_PREFIX, OPTIONS_4_SUFFIX)]
            public void OneResolveStep_OneKeyPerStep_FirstStepUnresolvable_ReturnsMapValueFunctionResult(
                SubstitutionOptions options, string prefix, string suffix
            )
            {
                // Arrange
                const string KEY_1 = "key1";
                const string KEY_2 = "key2";
                string VALUE_1 = prefix + KEY_2 + suffix;
                const string VALUE_1_MAPPED = "val1";

                configurationMock = Mock.Of<IConfiguration>(
                    c => c[KEY_1] == VALUE_1
                );

                var mapValueMock = Mock.Of<Func<string, string>>(
                    f => f(VALUE_1) == VALUE_1_MAPPED
                );

                // Act
                var result = configurationMock.ResolveValue(KEY_1, options, mapValueMock);

                // Assert
                Assert.AreEqual(VALUE_1_MAPPED, result);
            }

            [TestCase(OPTIONS_1, OPTIONS_1_PREFIX, OPTIONS_1_SUFFIX)]
            [TestCase(OPTIONS_2, OPTIONS_2_PREFIX, OPTIONS_2_SUFFIX)]
            [TestCase(OPTIONS_3, OPTIONS_3_PREFIX, OPTIONS_3_SUFFIX)]
            [TestCase(OPTIONS_4, OPTIONS_4_PREFIX, OPTIONS_4_SUFFIX)]
            public void OneResolveStep_TwoKeysPerStep_ReturnsResolved(
                SubstitutionOptions options, string prefix, string suffix
            )
            {
                // Arrange
                const string KEY_1 = "key1";
                const string KEY_2 = "key2";
                string VALUE_1 = prefix + KEY_2 + suffix + prefix + KEY_2 + suffix;
                const string VALUE_2 = "val2";

                configurationMock = Mock.Of<IConfiguration>(c =>
                    c[KEY_1] == VALUE_1 &&
                    c[KEY_2] == VALUE_2
                );

                // Act
                var actualValue = configurationMock.ResolveValue(KEY_1, options);

                // Assert
                Assert.AreEqual(VALUE_2 + VALUE_2, actualValue);
            }

            [TestCase(OPTIONS_1, OPTIONS_1_PREFIX, OPTIONS_1_SUFFIX)]
            [TestCase(OPTIONS_2, OPTIONS_2_PREFIX, OPTIONS_2_SUFFIX)]
            [TestCase(OPTIONS_3, OPTIONS_3_PREFIX, OPTIONS_3_SUFFIX)]
            [TestCase(OPTIONS_4, OPTIONS_4_PREFIX, OPTIONS_4_SUFFIX)]
            public void TwoResolveSteps_OneKeyPerStep_ReturnsResolved(
                SubstitutionOptions options, string prefix, string suffix
            )
            {
                // Arrange
                const string KEY_1 = "key1";
                const string KEY_2 = "key2";
                const string KEY_3 = "key3";
                string VALUE_1 = prefix + KEY_2 + suffix;
                string VALUE_2 = prefix + KEY_3 + suffix;
                const string VALUE_3 = "val2";

                configurationMock = Mock.Of<IConfiguration>(c =>
                    c[KEY_1] == VALUE_1 &&
                    c[KEY_2] == VALUE_2 &&
                    c[KEY_3] == VALUE_3
                );

                // Act
                var actualValue = configurationMock.ResolveValue(KEY_1, options);

                // Assert
                Assert.AreEqual(VALUE_3, actualValue);
            }

            [TestCase(OPTIONS_1, OPTIONS_1_PREFIX, OPTIONS_1_SUFFIX)]
            [TestCase(OPTIONS_2, OPTIONS_2_PREFIX, OPTIONS_2_SUFFIX)]
            [TestCase(OPTIONS_3, OPTIONS_3_PREFIX, OPTIONS_3_SUFFIX)]
            [TestCase(OPTIONS_4, OPTIONS_4_PREFIX, OPTIONS_4_SUFFIX)]
            public void TwoResolveSteps_OneKeyPerStep_SecondStepUnresolvable_InvokesMapValueFunction(
                SubstitutionOptions options, string prefix, string suffix
            )
            {
                // Arrange
                const string KEY_1 = "key1";
                const string KEY_2 = "key2";
                const string KEY_3 = "key3";
                string VALUE_1 = prefix + KEY_2 + suffix;
                string VALUE_2 = prefix + KEY_3 + suffix;

                configurationMock = Mock.Of<IConfiguration>(c =>
                    c[KEY_1] == VALUE_1 &&
                    c[KEY_2] == VALUE_2
                );

                var mapValueMock = Mock.Of<Func<string, string>>();

                // Act
                configurationMock.ResolveValue(KEY_1, options, mapValueMock);

                // Assert
                Mock.Get(mapValueMock)
                    .Verify(m => m(It.Is<string>(v => v == VALUE_2)));
            }

            [TestCase(OPTIONS_1, OPTIONS_1_PREFIX, OPTIONS_1_SUFFIX)]
            [TestCase(OPTIONS_2, OPTIONS_2_PREFIX, OPTIONS_2_SUFFIX)]
            [TestCase(OPTIONS_3, OPTIONS_3_PREFIX, OPTIONS_3_SUFFIX)]
            [TestCase(OPTIONS_4, OPTIONS_4_PREFIX, OPTIONS_4_SUFFIX)]
            public void InfiniteSteps_OneKeyPerStep_ThrowsValueUnresolvableException(
                SubstitutionOptions options, string prefix, string suffix
            )
            {
                // Arrange
                const string KEY_1 = "key1";
                string VALUE_1 = prefix + KEY_1 + suffix;

                configurationMock = Mock.Of<IConfiguration>(
                    c => c[KEY_1] == VALUE_1
                );

                // AssertThrows
                Assert.Throws<ValueUnresolvableException>(
                    () => configurationMock.ResolveValue(KEY_1, options)
                );
            }

            [TestCase(OPTIONS_1, OPTIONS_1_PREFIX, OPTIONS_1_SUFFIX)]
            [TestCase(OPTIONS_2, OPTIONS_2_PREFIX, OPTIONS_2_SUFFIX)]
            [TestCase(OPTIONS_3, OPTIONS_3_PREFIX, OPTIONS_3_SUFFIX)]
            [TestCase(OPTIONS_4, OPTIONS_4_PREFIX, OPTIONS_4_SUFFIX)]
            public void InfiniteSteps_TwoKeysPerStep_ThrowsValueUnresolvableException(
                SubstitutionOptions options, string prefix, string suffix
            )
            {
                // Arrange
                const string KEY_1 = "key1";
                string VALUE_1 = prefix + KEY_1 + suffix + prefix + KEY_1 + suffix;

                configurationMock = Mock.Of<IConfiguration>(
                    c => c[KEY_1] == VALUE_1
                );

                // AssertThrows
                Assert.Throws<ValueUnresolvableException>(
                    () => configurationMock.ResolveValue(KEY_1, options)
                );
            }

            [TestCase(OPTIONS_1, OPTIONS_1_PREFIX, OPTIONS_1_SUFFIX)]
            [TestCase(OPTIONS_2, OPTIONS_2_PREFIX, OPTIONS_2_SUFFIX)]
            [TestCase(OPTIONS_3, OPTIONS_3_PREFIX, OPTIONS_3_SUFFIX)]
            [TestCase(OPTIONS_4, OPTIONS_4_PREFIX, OPTIONS_4_SUFFIX)]
            public void KeyNotExists_ThrowsKeyNotFoundException(
                SubstitutionOptions options, string prefix, string suffix
            )
            {
                Assert.Throws<KeyNotFoundException>(
                    () => configurationMock.ResolveValue(prefix + suffix, options)
                );
            }

            [Test]
            public void ParameterKeyNull_ThrowsArgumentNullException()
            {
                Assert.Throws<ArgumentNullException>(
                    () => configurationMock.ResolveValue(null)
               );
            }

            [Test]
            public void ParameterMapValueNull_ThrowsArgumentNullException()
            {
                Assert.Throws<ArgumentNullException>(
                    () => configurationMock.ResolveValue(
                        "KEY", SubstitutionOptions.All, null
                    )
               );
            }
        }
    }
}