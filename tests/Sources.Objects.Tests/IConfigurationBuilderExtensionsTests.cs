using System;
using System.Collections.Generic;
using System.Reflection;
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
        public void Test_AddObject_Tuple_DoesNotThrow()
        {
            // Act & Assert
            Assert.DoesNotThrow(
                () => builderMock.AddObject(("val1", "val2"))
            );
        }

        [Test]
        public void Test_AddObject_NamedTuple_DoesNotThrow()
        {
            // Act & Assert
            Assert.DoesNotThrow(
                () => builderMock.AddObject((p1: "val1", p2: "val2"))
            );
        }

        [Test]
        public void Test_AddObject_AnonymousType_DoesNotThrow()
        {
            // Act & Assert
            Assert.DoesNotThrow(
                () => builderMock.AddObject(new { p1 = "val1" })
            );
        }
    }
}