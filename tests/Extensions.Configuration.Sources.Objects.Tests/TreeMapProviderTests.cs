using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Extensions.Configuration.Sources.Objects.Tests
{
    [TestFixture]
    public class TreeMapProviderTests
    {
        [Test]
        public void Test_GetTreeMap_ObjectWithStringProperties_PropertiesAdded()
        {
            // Arrange
            const string FIRST_NAME = "Jan";
            const string LAST_NAME = "Donnermayer";

            var source = new { FirstName = FIRST_NAME, LastName = LAST_NAME };

            var expectedProperties = new Dictionary<string, object>()
            {
                { "FirstName" , FIRST_NAME },
                { "LastName", LAST_NAME }
            };

            // Act
            var properties = TreeMapProvider.GetTreeMap(source);

            // Assert
            Assert.AreEqual(expectedProperties, properties);
        }

        [Test]
        public void Test_GetTreeMap_ObjectWithValueProperties_PropertiesAdded()
        {
            // Arrange
            const int AGE = 24;
            const double HEIGHT = 1.75;
            const bool MALE = true;

            var source = new { Age = AGE, Height = HEIGHT, Male = MALE };

            var expectedProperties = new Dictionary<string, object>()
            {
                { "Age" , AGE },
                { "Height", HEIGHT },
                { "Male", MALE },
            };

            // Act
            var properties = TreeMapProvider.GetTreeMap(source);

            // Assert
            Assert.AreEqual(expectedProperties, properties);
        }

        [Test]
        public void Test_GetTreeMap_NestedObject_StringPropertiesAdded()
        {
            // Arrange
            const string NAME = "Jan";
            const string COMPANY_NAME = "Genet";

            var source = new { Boss = new { Name = NAME }, CompanyName = COMPANY_NAME };

            var expectedBoss = new Dictionary<string, object>()
            {
                { "Name", NAME }
            };

            var expectedProperties = new Dictionary<string, object>()
            {
                { "CompanyName" , COMPANY_NAME },
                { "Boss", expectedBoss }
            };

            // Act
            var properties = TreeMapProvider.GetTreeMap(source);

            // Assert
            Assert.AreEqual(expectedProperties, properties);
        }

        [Test]
        public void Test_GetTreeMap_RecursiveObject_ThrowsInvalidOperationException()
        {
            // Arrange
            var x = new Box<object>();
            x.Value = x;

            // Act & Assert
            Assert.Throws<InvalidOperationException>(
                () => TreeMapProvider.GetTreeMap(x)
            );
        }

        [Test]
        public void Test_GetTreeMap_WriteOnlyObject_DoesNotThrow()
        {
            // Arrange & Act & Assert
            Assert.DoesNotThrow(
                () => TreeMapProvider.GetTreeMap(new WriteOnlyBox<object>())
            );
        }

        [Test]
        public void Test_GetTreeMap_IndexerObject_DoesNotThrow()
        {
            // Arrange & Act & Assert
            Assert.DoesNotThrow(
                () => TreeMapProvider.GetTreeMap(new IndexerBox<object>())
            );
        }

        private class Box<T> { public T Value { get; set; } }

        private class WriteOnlyBox<T> { public T Value { set { } } }

        private class IndexerBox<T> { public T this[T arg] => arg; }
    }
}