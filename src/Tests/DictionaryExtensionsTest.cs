using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cloud.Core.Attributes;
using Cloud.Core.Extensions;
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

            dictionary.AddOrUpdate("Property4", "PropValue");
            dictionary.AddOrUpdate("Property4", "PropValue4");
            dictionary.Keys.Count.Should().Be(4);

            dictionary["Property1"].Should().Be("TEST1");
            dictionary.ContainsKey("Property1").Should().BeTrue();
            dictionary.ContainsKey("Property2").Should().BeTrue();
            dictionary.ContainsKey("Property3").Should().BeTrue();
            dictionary.ContainsKey("Property4").Should().BeTrue();
        }

        /// <summary>Check a value can be added to or updated if already exists.</summary>
        [Fact]
        public void Test_IDictionary_AddOrUpdateValue()
        {
            // Arrange/Act
            IDictionary<string,string> dictionary = new Dictionary<string, string> {
                { "Property1", "PropValue1" },
                { "Property2", "PropValue2" },
                { "Property3", "PropValue3" }
            };

            // Assert
            dictionary["Property1"].Should().Be("PropValue1");
            dictionary.AddOrUpdate(new KeyValuePair<string, string>("Property1", "TEST1"));
            dictionary.Keys.Count.Should().Be(3);

            dictionary.AddOrUpdate("Property4", "PropValue");
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
                PropC = true,
                PropD = null,
                PropZ = null
            };

            // Act
            var dictionary = objTest.AsDictionary();
            
            // Assert
            dictionary.Keys.Count.Should().Be(5);
            dictionary.ContainsKey("PropA").Should().BeTrue();
            dictionary.ContainsKey("PropB").Should().BeTrue();
            dictionary.ContainsKey("PropC").Should().BeTrue();
        }

        /// <summary>Check dictionary can be converted into a list of key value pairs.</summary>
        [Fact]
        public void Test_Dictionary_ToList()
        {
            // Arrange
            var dictionarySource = new Dictionary<string, object>
            {
                { "A", 1 },
                { "B", 2 },
                { "C", 3 }
            };

            // Act
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
            var dictionarySource = new Dictionary<string, object>
            {
                { "A", 1 },
                { "B", 2 },
                { "C", 3 }
            };

            // Act
            var list = dictionarySource.ToArray();

            // Assert
            list[0].Value.Should().Be(1);
            list[1].Value.Should().Be(2);
            list[2].Value.Should().Be(3);
        }

        /// <summary>Verify the conversion of an object to dictionary (string key/object) works as expected.</summary>
        [Fact]
        public void Test_Object_ToFlatStringDictionary()
        {
            // Arrange
            var objTest = new Test
            {
                PropA = "test1",
                PropB = 1,
                PropC = true,
                PropD = new SubItem
                {
                    PropE = "test2",
                    PropF = new List<int> { 1, 2, 3 }
                },
                PropZ = new Test
                {
                    PropA = "test3",
                    PropB = 1,
                    PropC = true,
                    PropD = new SubItem
                    {
                        PropE = "test4",
                        PropF = new List<int> { 1, 1, 1 }
                    }
                },
                Password = "Password123",
                Name = "Robert",
                PhoneNumber = ""
            };

            // Act
            var result = objTest.AsFlatStringDictionary();
            var lowercaseResult = objTest.AsFlatDictionary(StringCasing.Lowercase, true);
            var uppercaseResult = objTest.AsFlatDictionary(StringCasing.Uppercase);

            // Assert
            result.Should().NotBeNull();
            result["PropA"].Should().Be("test1");
            result["PropB"].Should().Be("1");
            result["PropC"].Should().Be("True");
            result["PropD:PropE"].Should().Be("test2");
            result["PropD:PropF[0]"].Should().Be("1");
            result["PropD:PropF[1]"].Should().Be("2");
            result["PropD:PropF[2]"].Should().Be("3");
            result["PropZ:PropA"].Should().Be("test3");
            result["PropZ:PropB"].Should().Be("1");
            result["PropZ:PropC"].Should().Be("True");
            result["PropZ:PropD:PropE"].Should().Be("test4");
            result["PropZ:PropD:PropF[0]"].Should().Be("1");
            result["PropZ:PropD:PropF[1]"].Should().Be("1");
            result["PropZ:PropD:PropF[2]"].Should().Be("1");
            lowercaseResult["propa"].Should().Be("test1");
            lowercaseResult["password"].Should().Be("*****");
            lowercaseResult["phonenumber"].Should().Be("");
            uppercaseResult["PASSWORD"].Should().NotBe("*****");
        }

        /// <summary>Verify the conversion of an object to dictionary works as expected.</summary>
        [Fact]
        public void Test_Object_ToFlatDictionary()
        {
            // Arrange
            object objTest = new Test
            {
                PropA = "test1",
                PropB = 1,
                PropC = true,
                PropD = new SubItem
                {
                    PropE = "test2",
                    PropF = new List<int> { 1, 2, 3 }
                },
                PropZ = new Test
                {
                    PropA = "test3",
                    PropB = 1,
                    PropC = true,
                    PropD = new SubItem
                    {
                        PropE = "test4",
                        PropF = new List<int> { 1, 1, 1 }
                    }
                }
            };

            // Act
            var result = objTest.AsFlatDictionary();
            var lowercaseResult = objTest.AsFlatDictionary(StringCasing.Lowercase);
            var uppercaseResult = objTest.AsFlatDictionary(StringCasing.Uppercase);

            // Assert
            result.Should().NotBeNull();
            result.Keys.Contains("PropA").Should().BeTrue();
            result.Keys.Contains("PropB").Should().BeTrue();
            result.Keys.Contains("PropC").Should().BeTrue();
            result.Keys.Contains("PropD:PropE").Should().BeTrue();
            result.Keys.Contains("PropD:PropF[0]").Should().BeTrue();
            result.Keys.Contains("PropD:PropF[1]").Should().BeTrue();
            result.Keys.Contains("PropD:PropF[2]").Should().BeTrue();
            result.Keys.Contains("PropZ:PropA").Should().BeTrue();
            result.Keys.Contains("PropZ:PropB").Should().BeTrue();
            result.Keys.Contains("PropZ:PropC").Should().BeTrue();
            result.Keys.Contains("PropZ:PropD:PropE").Should().BeTrue();
            result.Keys.Contains("PropZ:PropD:PropF[0]").Should().BeTrue();
            result.Keys.Contains("PropZ:PropD:PropF[1]").Should().BeTrue();
            result.Keys.Contains("PropZ:PropD:PropF[2]").Should().BeTrue();
            lowercaseResult.Keys.Contains("propa").Should().BeTrue();
            uppercaseResult.Keys.Contains("PROPA").Should().BeTrue();
        }

        /// <summary>Check dictionary can be converted into an object.</summary>
        [Fact]
        public void Test_Dictionary_ToObject()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>
            {
                { "PropA", "test" },
                { "PropB", 1 },
                { "PropC", true },
                { "PropD", new SubItem {
                        PropE = "PropE",
                        PropF = new List<int> { 1, 2, 3 }
                    } 
                }
            };
            var objToDictionary = new Test
            {
                PropA = "test",
                PropB = 1,
                PropC = true,
                PropD = new SubItem
                {
                    PropE = "PropE",
                    PropF = new List<int> { 1, 2, 3 }
                }
            };

            // Act
            var objTest = dictionary.ToObject<Test>();
            var dict = objToDictionary.AsDictionary();

            // Assert
            objTest.PropA.Should().Be("test");
            objTest.PropB.Should().Be(1);
            objTest.PropC.Should().BeTrue();

            dict.Keys.Contains("PropA").Should().BeTrue();
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
            public SubItem PropD { get; set; }
            public Test PropZ { get; set; }

            [PersonalData]
            public string Name { get; set; }
            [PersonalData]
            public DateTime Dob { get; set; }
            [PersonalData]
            public string PhoneNumber { get; set; }
            [SensitiveInfo]
            public string Password { get; set; }
        }

        public class SubItem
        {
            public string PropE { get; set; }
            public List<int> PropF { get; set; }
        }
    }
}
