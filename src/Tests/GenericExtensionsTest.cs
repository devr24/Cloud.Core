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
        /// <summary>Verify object type is changed as expected.</summary>
        [Fact]
        public void Test_Object_ChangeType()
        {
            // Arrange
            var mytest = "0";

            // Act
            var changed = mytest.ChangeType<int>();

            // Assert
            changed.GetType().Should().Be(typeof(int));
        }

        /// <summary>Verify list object is null or default check works as expected.</summary>
        [Fact]
        public void Test_ListObject_IsNullOrDefault()
        {
            // Arrange
            List<string> obj = null;

            // Act
            var result = obj.IsNullOrDefault();

            // Assert
            Assert.True(result);
        }

        /// <summary>Verify object is null or default check works as expected.</summary>
        [Fact]
        public void Test_NullObject_IsNullOrDefault()
        {
            // Arrange
            object obj = null;

            // Act
            var result = obj.IsNullOrDefault();

            // Assert
            Assert.True(result);
        }

        /// <summary>Verify non-null list is null or default check works as expected.</summary>
        [Fact]
        public void Test_NonNullList_IsNullOrDefault()
        {
            // Arrange
            var obj = new List<string> { "test" };

            // Act
            var result = obj.IsNullOrDefault();

            // Assert
            Assert.False(result);
        }

        /// <summary>Verify non-null string is null or default check works as expected.</summary>
        [Fact]
        public void Test_StringObject_IsNullOrDefault()
        {
            // Arrange
            object obj = "string";

            // Act
            var result = obj.IsNullOrDefault();

            // Assert
            Assert.False(result);
        }

        /// <summary>Verify null list throws if null or default works as expected.</summary>
        [Fact]
        public void Test_NullList_ThrowIfNullOrDefault()
        {
            // Act
            List<string> obj = null;

            // Act/Assert
            Assert.Throws<ArgumentNullException>(() => obj.ThrowIfNullOrDefault());
        }

        /// <summary>Verify null object throws if null or default works as expected.</summary>
        [Fact]
        public void Test_NullObject_ThrowIfNullOrDefault()
        {
            // Act
            object obj = null;

            // Act/Assert
            Assert.Throws<ArgumentNullException>(() => obj.ThrowIfNullOrDefault());
        }

        /// <summary>Verify a sub array can be taken from a source array.</summary>
        [Fact]
        public void Test_Array_SubArray()
        {
            // Arrange
            var testData = new string[] { "1", "2", "3", "4", "5" };

            // Act
            var subArray = testData.SubArray(0, 3);

            // Assert
            subArray.Length.Should().Be(3);
            subArray[0].Should().Be("1");
            subArray[1].Should().Be("2");
            subArray[2].Should().Be("3");
        }

        /// <summary>Verify an object can be converted into a list of property desciptions.</summary>
        [Fact]
        public void Test_PropertyDescription_ToList()
        {
            // Arrange
            var testData = new TestClass { 
                PropA = "PropAVal",
                PropB = "PropBVal",
                PropC = new List<int> { 1,2,3 },
                PropD = new int[] { 1, 2, 3 },
                PropE = new List<int> { 1, 2, 3 }
            };

            // Act
            var props = testData.GetPropertyDescription().ToList();

            // Assert
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
