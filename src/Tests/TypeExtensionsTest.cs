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
    public class TypeExtensionsTest
    {
        /// <summary>Verify object type is or is not a system type as expected.</summary>
        [Fact]
        public void Test_Type_IsSystemType()
        {
            // Arrange
            var testData = new TestClass() { PropA = "test", PropB = "test" };

            // Act
            var propA = testData.PropA.GetType().IsSystemType();
            var propB = testData.PropB.GetType().IsSystemType();
            var propOther = testData.GetType().IsSystemType();

            // Assert
            propA.Should().BeTrue();
            propB.Should().BeTrue();
            propOther.Should().BeFalse();
        }

        /// <summary>Verify list of property info generated matches expected types.</summary>
        [Fact]
        public void Test_Type_GetRequiredProperties()
        {
            // Arrange
            var testData = new TestClass
            {
                PropA = "PropAVal",
                PropB = "PropBVal",
                PropC = new List<int> { 1, 2, 3 },
                PropD = new int[] { 1, 2, 3 },
                PropE = new List<int> { 1, 2, 3 }
            };

            // Act
            var requiredProps = typeof(TestClass).GetRequiredProperties().ToList();

            // Assert
            requiredProps.Count.Should().Be(3);
            requiredProps[0].PropertyType.Should().Be(testData.PropA.GetType());
            requiredProps[1].PropertyType.Should().Be(testData.PropD.GetType());
            requiredProps[2].PropertyType.IsAssignableFrom(testData.PropE.GetType()).Should().BeTrue();
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
            public TestOther PropOther { get; set; }
        }

        private class TestOther { }
    }
}
