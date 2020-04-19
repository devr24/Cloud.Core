using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Cloud.Core.Testing;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Cloud.Core.Tests
{
    [IsUnit]
    public class GenericExtensionsTest
    {
        [Fact]
        public void Test_ChangeType()
        {
            var mytest = "0";
            var changed = mytest.ChangeType<int>();
            changed.GetType().Should().Be(typeof(int));
        }

        [Fact]
        public void Test_IsNullOrDefault_NullList()
        {
            List<string> obj = null;
            Assert.True(obj.IsNullOrDefault());
        }

        [Fact]
        public void Test_IsNullOrDefault_NullObj()
        {
            object obj = null;
            Assert.True(obj.IsNullOrDefault());
        }

        [Fact]
        public void Test_IsNullOrDefault_NullList_Negative()
        {
            var obj = new List<string> { "test" };
            Assert.False(obj.IsNullOrDefault());
        }

        [Fact]
        public void Test_IsNullOrDefault_NullObj_Negative()
        {
            object obj = "string";
            Assert.False(obj.IsNullOrDefault());
        }

        [Fact]
        public void Test_ThrowIfNullOrDefault_ThrowNullList()
        {
            List<string> obj = null;
            Assert.Throws<ArgumentNullException>(() => obj.ThrowIfNullOrDefault());
        }

        [Fact]
        public void Test_ThrowIfNullOrDefault_ThrowNullObj()
        {
            object obj = null;
            Assert.Throws<ArgumentNullException>(() => obj.ThrowIfNullOrDefault());
        }

        [Fact]
        public void Test_SubArray()
        {
            var testData = new string[] { "1", "2", "3", "4", "5" };
            var subArray = testData.SubArray(0, 3);
            subArray.Length.Should().Be(3);
            subArray[0].Should().Be("1");
            subArray[1].Should().Be("2");
            subArray[2].Should().Be("3");
        }

        [Fact]
        public void Test_PropertyDescription()
        {
            var testData = new TestClass { 
                PropA = "PropAVal",
                PropB = "PropBVal",
                PropC = new List<int> { 1,2,3 },
                PropD = new int[] { 1, 2, 3 },
                PropE = new List<int> { 1, 2, 3 }
            };

            var props = testData.GetPropertyDescription().ToList();
            props.Count.Should().Be(5);
            
            props[0].Name.Should().Be("PropA");
            props[0].Value.Should().Be(testData.PropA);
            props[0].HasKeyAttribute.Should().Be(false);
            props[0].HasRequiredAttribute.Should().Be(true);
            props[0].IsEnumerable.Should().Be(false);
            props[0].Type.Should().Be(typeof(string));

            props[1].Value.Should().Be(testData.PropB);
            props[1].Name.Should().Be("PropB");
            props[1].HasKeyAttribute.Should().Be(true);
            props[1].IsEnumerable.Should().Be(false);
            props[1].HasRequiredAttribute.Should().Be(false);
            props[1].Type.Should().Be(typeof(string));

            props[2].Value.Should().Be(testData.PropC);
            props[2].Name.Should().Be("PropC");
            props[2].HasKeyAttribute.Should().Be(false);
            props[2].IsEnumerable.Should().Be(true);
            props[2].HasRequiredAttribute.Should().Be(false);
            props[2].Type.Should().Be(typeof(List<int>));

            props[3].Value.Should().Be(testData.PropD);
            props[3].Name.Should().Be("PropD");
            props[3].HasKeyAttribute.Should().Be(false);
            props[3].IsEnumerable.Should().Be(true);
            props[3].HasRequiredAttribute.Should().Be(true);
            props[3].Type.Should().Be(typeof(int[]));

            props[4].Value.Should().Be(testData.PropE);
            props[4].Name.Should().Be("PropE");
            props[4].HasKeyAttribute.Should().Be(false);
            props[4].IsEnumerable.Should().Be(true);
            props[4].HasRequiredAttribute.Should().Be(true);
            props[4].Type.Should().Be(typeof(IEnumerable<int>));
        }

        private class TestClass { 
            [Required]
            public string PropA { get; set; }
            [Key]
            public string PropB { get; set; }
            [JsonProperty(Required = Required.AllowNull)]
            public List<int> PropC { get; set; }
            [JsonProperty(Required = Required.Always)]
            public int[] PropD { get; set; }
            [JsonRequired]
            public IEnumerable<int> PropE { get; set; }
        }
    }
}
