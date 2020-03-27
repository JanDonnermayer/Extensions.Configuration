using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace Extensions.Configuration.Sources.Object.Tests
{
    public class ObjectToDictionaryConverterTests
    {

        [Test]
        public void Test_StringPropertiesAdded()
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
            var properties = DictionaryProvider.GetDictionary(source);

            // Assert
            Assert.AreEqual(expectedProperties, properties);
        }

        [Test]
        public void Test_ValuePropertiesAdded()
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
            var properties = DictionaryProvider.GetDictionary(source);

            // Assert
            Assert.AreEqual(expectedProperties, properties);
        }

        [Test]
        public void Test_Nested_StringPropertiesAdded()
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
            var properties = DictionaryProvider.GetDictionary(source);

            // Assert
            Assert.AreEqual(expectedProperties, properties);
        }

        [Test]
        public void Test_Recursive_ThrowsInvalidOperationException()
        {
            // Setup
            var x = new Box<object>();
            x.Value = x;

            // Act & Assert
            Assert.Throws<InvalidOperationException>(
                () => DictionaryProvider.GetDictionary(x)
            );
        }

        [Test]
        public void Test_WriteOnlyObject_DoesNotThrow()
        {
            // Setup & Act & Assert
            Assert.DoesNotThrow(
                () => DictionaryProvider.GetDictionary(new WriteOnlyBox<object>())
            );
        }

        private class Box<T> { public T Value { get; set; } }

        private class WriteOnlyBox<T> { public T Value { set { } } }
    }
}