﻿using System.Collections.Generic;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;

namespace Extensions.Configuration.Sources.Objects.Tests
{
    [TestFixture]
    public class MapConfigurationProviderTests
    {
        [Test]
        public void Test_FromKVPEntries_TryGetValue()
        {
            // Arrange
            var source = new Dictionary<string, string>()
            {
                { "K1",  "V1" }
            };

            // Act
            var result = MapConfigurationProvider
                .FromEntries(source)
                .TryGetValue("K1");

            // Assert
            Assert.AreEqual((true, "V1"), result);
        }

        [Test]
        public void Test_FromTupleEntries_TryGetValue()
        {
            // Arrange
            var source = new []
            {
                ("K1",  "V1")
            };

            // Act
            var result = MapConfigurationProvider
                .FromEntries(source)
                .TryGetValue("K1");

            // Assert
            Assert.AreEqual((true, "V1"), result);
        }
    }
}