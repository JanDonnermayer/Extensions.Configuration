using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Extensions.Configuration.Sources.Objects.Tests
{
    [TestFixture]
    public class DictionaryProviderTests
    {
        [Test]
        public void Test_GetDictionary_ObjectWithStringProperties_PropertiesAdded()
        {
            // Setup
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
        public void Test_GetDictionary_ObjectWithValueProperties_PropertiesAdded()
        {
            // Setup
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
        public void Test_GetDictionary_NestedObject_StringPropertiesAdded()
        {
            // Setup
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
        public void Test_GetDictionary_RecursiveObject_ThrowsInvalidOperationException()
        {
            // Setup
            var x = new Box<object>();
            x.Value = x;

            // Act & Assert
            Assert.Throws<InvalidOperationException>(
                () => TreeMapProvider.GetTreeMap(x)
            );
        }

        [Test]
        public void Test_GetDictionary_WriteOnlyObject_DoesNotThrow()
        {
            // Setup & Act & Assert
            Assert.DoesNotThrow(
                () => TreeMapProvider.GetTreeMap(new WriteOnlyBox<object>())
            );
        }

        private class Box<T> { public T Value { get; set; } }

        private class WriteOnlyBox<T> { public T Value { set { } } }
    }
}