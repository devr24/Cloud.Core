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
        private class Test
        {
            public string PropA { get; set; }
            public int PropB { get; set; }
            public bool PropC { get; set; }
        }

        [Fact]
        public void Test_Dictionary_UpdateOrCreate()
        {
            var dictionary = new Dictionary<string, string> {
                { "Property1", "PropValue1" },
                { "Property2", "PropValue2" },
                { "Property3", "PropValue3" }
            };

            dictionary["Property1"].Should().Be("PropValue1");
            dictionary.AddOrUpdate("Property1", "TEST1");
            dictionary.Keys.Count.Should().Be(3);

            dictionary.AddOrUpdate("Property4", "PropValue4");
            dictionary.Keys.Count.Should().Be(4);

            dictionary["Property1"].Should().Be("TEST1");
            dictionary.ContainsKey("Property1").Should().BeTrue();
            dictionary.ContainsKey("Property2").Should().BeTrue();
            dictionary.ContainsKey("Property3").Should().BeTrue();
            dictionary.ContainsKey("Property4").Should().BeTrue();
        }

        [Fact]
        public void Test_IDictionary_UpdateOrCreate()
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string> {
                { "Property1", "PropValue1" },
                { "Property2", "PropValue2" },
                { "Property3", "PropValue3" }
            };

            dictionary["Property1"].Should().Be("PropValue1");
            dictionary.AddOrUpdate("Property1", "TEST1");
            dictionary.Keys.Count.Should().Be(3);

            dictionary.AddOrUpdate("Property4", "PropValue4");
            dictionary.Keys.Count.Should().Be(4);

            dictionary["Property1"].Should().Be("TEST1");
            dictionary.ContainsKey("Property1").Should().BeTrue();
            dictionary.ContainsKey("Property2").Should().BeTrue();
            dictionary.ContainsKey("Property3").Should().BeTrue();
            dictionary.ContainsKey("Property4").Should().BeTrue();
        }

        [Fact]
        public void Test_IDictionary_UpdateOrCreate_keyvaluepair()
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string> {
                { "Property1", "PropValue1" },
                { "Property2", "PropValue2" },
                { "Property3", "PropValue3" }
            };

            dictionary["Property1"].Should().Be("PropValue1");
            dictionary.AddOrUpdate(new KeyValuePair<string,string>("Property1", "TEST1"));
            dictionary.Keys.Count.Should().Be(3);

            dictionary.AddOrUpdate("Property4", "PropValue4");
            dictionary.Keys.Count.Should().Be(4);

            dictionary["Property1"].Should().Be("TEST1");
            dictionary.ContainsKey("Property1").Should().BeTrue();
            dictionary.ContainsKey("Property2").Should().BeTrue();
            dictionary.ContainsKey("Property3").Should().BeTrue();
            dictionary.ContainsKey("Property4").Should().BeTrue();
        }

        [Fact]
        public void Test_Dictionary_UpdateOrCreate_keyvaluepair()
        {
            var dictionary = new Dictionary<string, string> {
                { "Property1", "PropValue1" },
                { "Property2", "PropValue2" },
                { "Property3", "PropValue3" }
            };

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

        [Fact]
        public void Test_ObjectToDictionary()
        {
            var objTest = new Test {
                PropA = "test",
                PropB = 1,
                PropC = true
            };

            var dictionary = objTest.AsDictionary();
            dictionary.Keys.Count.Should().Be(3);
            dictionary.ContainsKey("PropA").Should().BeTrue();
            dictionary.ContainsKey("PropB").Should().BeTrue();
            dictionary.ContainsKey("PropC").Should().BeTrue();
        }

        [Fact]
        public void Test_DictionaryToList()
        {
            var dictionarySource = new Dictionary<string, object>();
            dictionarySource.Add("A", 1);
            dictionarySource.Add("B", 2);
            dictionarySource.Add("C", 3);

            var list = dictionarySource.ToList();

            list.ElementAt(0).Value.Should().Be(1);
            list.ElementAt(1).Value.Should().Be(2);
            list.ElementAt(2).Value.Should().Be(3);
        }

        [Fact]
        public void Test_DictionaryToArray()
        {
            var dictionarySource = new Dictionary<string, object>();
            dictionarySource.Add("A", 1);
            dictionarySource.Add("B", 2);
            dictionarySource.Add("C", 3);

            var list = dictionarySource.ToArray();

            list[0].Value.Should().Be(1);
            list[1].Value.Should().Be(2);
            list[2].Value.Should().Be(3);
        }

        [Fact]
        public void Test_DictionaryToObject()
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add("PropA", "test");
            dictionary.Add("PropB", 1);
            dictionary.Add("PropC", true);

            var objTest = dictionary.ToObject<Test>();
            objTest.PropA.Should().Be("test");
            objTest.PropB.Should().Be(1);
            objTest.PropC.Should().BeTrue();
        }

        [Fact]
        public void Test_Append_Dictionary()
        {
            // Arrange
            var source = new Dictionary<string, object>
            {
                { "one", 1 }
            };
            var rangeToAdd = new Dictionary<string, object>
            {
                { "two", 2 },
                { "three", 3 }
            };

            // Act
            source.AddRange(rangeToAdd);

            // Assert
            source.Count.Should().Be(3);
        }

        [Fact]
        public void Test_Append_KeyValuePair()
        {
            // Arrange
            var source = new Dictionary<string, object>
            {
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

        [Fact]
        public void Test_Release()
        {
            var testDict = new Dictionary<string, MemoryStream>
            {
                { "1", new MemoryStream() },
                { "2", new MemoryStream() }
            };

            Assert.Equal(2, testDict.Count);

            testDict.Release();

            Assert.Empty(testDict);
        }

        [Fact]
        public void Test_ToString()
        {
            var testDict = new Dictionary<string, string>
            {
                { "1", "1" },
                { "2", "2" }
            };

            Assert.Equal("1=1;2=2", testDict.ToDelimitedString());
        }
    }
}
