using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cloud.Core.Testing;
using FluentAssertions;
using Xunit;

namespace Cloud.Core.Tests
{
    [IsUnit]
    public class DictionaryExtensionsTest
    {
        /// <summary>Check a value can be added to or updated if already exists.</summary>
        [Fact]
        public void Test_Dictionary_AddOrUpdateValue()
        {
            // Arrange/Act
            var dictionary = new Dictionary<string, string> {
                { "Property1", "PropValue1" },
                { "Property2", "PropValue2" },
                { "Property3", "PropValue3" }
            };

            // Assert
            dictionary["Property1"].Should().Be("PropValue1");
            dictionary.AddOrUpdate(new KeyValuePair<string, string>("Property1", "TEST1"));
            dictionary.Keys.Count.Should().Be(3);

            dictionary.AddOrUpdate("Property4", "PropValue4");
            dictionary.Keys.Count.Should().Be(4);

            dictionary["Property1"].Should().Be("TEST1");
            dictionary.ContainsKey("Property1").Should().BeTrue();
            dictionary.ContainsKey("Property2").Should().BeTrue();
            dictionary.ContainsKey("Property3").Should().BeTrue();
            dictionary.ContainsKey("Property4").Should().BeTrue();
        }

        /// <summary>Check an object can be converted into a dictionary.</summary>
        [Fact]
        public void Test_Dictionary_FromObject()
        {
            // Arrange
            var objTest = new Test {
                PropA = "test",
                PropB = 1,
                PropC = true
            };

            // Act
            var dictionary = objTest.AsDictionary();
            
            // Assert
            dictionary.Keys.Count.Should().Be(3);
            dictionary.ContainsKey("PropA").Should().BeTrue();
            dictionary.ContainsKey("PropB").Should().BeTrue();
            dictionary.ContainsKey("PropC").Should().BeTrue();
        }

        /// <summary>Check dictionary can be converted into a list of key value pairs.</summary>
        [Fact]
        public void Test_Dictionary_ToList()
        {
            // Arrange
            var dictionarySource = new Dictionary<string, object>();
            
            // Act
            dictionarySource.Add("A", 1);
            dictionarySource.Add("B", 2);
            dictionarySource.Add("C", 3);

            var list = dictionarySource.ToList();

            // Assert
            list.ElementAt(0).Value.Should().Be(1);
            list.ElementAt(1).Value.Should().Be(2);
            list.ElementAt(2).Value.Should().Be(3);
        }

        /// <summary>Check dictionary can converted into an array.</summary>
        [Fact]
        public void Test_Dictionary_ToArray()
        {
            // Arrange
            var dictionarySource = new Dictionary<string, object>();
            
            // Act
            dictionarySource.Add("A", 1);
            dictionarySource.Add("B", 2);
            dictionarySource.Add("C", 3);

            var list = dictionarySource.ToArray();

            // Assert
            list[0].Value.Should().Be(1);
            list[1].Value.Should().Be(2);
            list[2].Value.Should().Be(3);
        }

        /// <summary>Check dictionary can be converted into an object.</summary>
        [Fact]
        public void Test_Dictionary_ToObject()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>();
            
            // Act
            dictionary.Add("PropA", "test");
            dictionary.Add("PropB", 1);
            dictionary.Add("PropC", true);

            var objTest = dictionary.ToObject<Test>();
            
            // Assert
            objTest.PropA.Should().Be("test");
            objTest.PropB.Should().Be(1);
            objTest.PropC.Should().BeTrue();
        }

        /// <summary>Check dictionary can have all elements from an existing dictionary appended on.</summary>
        [Fact]
        public void Test_Dictionary_AddRange_ValuesFromDictionary()
        {
            // Arrange
            var source = new Dictionary<string, object> {
                { "one", 1 }
            };
            var rangeToAdd = new Dictionary<string, object> {
                { "two", 2 },
                { "three", 3 }
            };

            // Act
            source.AddRange(rangeToAdd);

            // Assert
            source.Count.Should().Be(3);
        }

        /// <summary>Check dictionary can have an array of key value pairs appended on.</summary>
        [Fact]
        public void Test_Dictionary_AddRange_ValuesFromKeyValuePair()
        {
            // Arrange
            var source = new Dictionary<string, object> {
                { "one", 1 }
            };
            var rangeToAdd = new KeyValuePair<string, object>[2];
            rangeToAdd[0] = new KeyValuePair<string, object>("two", 2);
            rangeToAdd[1] = new KeyValuePair<string, object>("three", 3);

            // Act
            source.AddRange(rangeToAdd);

            // Assert
            source.Count.Should().Be(3);
        }

        /// <summary>Check the release method clears down the dictionary and releases all disposable objects.</summary>
        [Fact]
        public void Test_Dictionary_Release()
        {
            // Arrange
            var testDict = new Dictionary<string, MemoryStream> {
                { "1", new MemoryStream() },
                { "2", new MemoryStream() }
            };

            // Act
            var beforeCount = testDict.Count;
            testDict.Release();
            var afterCount = testDict.Count;

            // Assert
            Assert.True(beforeCount > afterCount);
            Assert.Equal(0, afterCount);
        }

        /// <summary>Check dictionary is converted into a delimted string as expected using default delimiter.</summary>
        [Fact]
        public void Test_Dictionary_ToStringDefault()
        {
            // Arrange
            var testDict = new Dictionary<string, string> {
                { "1", "1" },
                { "2", "2" }
            };

            // Act
            var delimited = testDict.ToDelimitedString();

            // Assert
            Assert.Equal("1=1;2=2", delimited);
        }

        /// <summary>Check dictionary is converted into a delimted string as expected using default delimiter.</summary>
        [Fact]
        public void Test_Dictionary_ToString()
        {
            // Arrange
            var testDict = new Dictionary<string, string> {
                { "1", "1" },
                { "2", "2" }
            };

            // Act
            var delimited = testDict.ToDelimitedString("|");

            // Assert
            Assert.Equal("1=1|2=2", delimited);
        }

        private class Test
        {
            public string PropA { get; set; }
            public int PropB { get; set; }
            public bool PropC { get; set; }
        }
    }
}
