﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Cloud.Core.Attributes;
using Cloud.Core.Extensions;
using Cloud.Core.Testing;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Cloud.Core.Tests
{
    [IsUnit]
    public class TypeExtensionsTest
    {
        [Fact]
        public void Test_Type_GetNamedAttributeProperties()
        {
            // Arrange
            var testData = new NamedAttExample { Property1 = "test", Property2 = "test" };

            // Act
            var attributes = testData.GetNamedAttributeProperties("RobTest1");

            // Assert
            attributes.Should().NotBeEmpty();
            attributes.FirstOrDefault().Key.Should().Be("Property1");
            attributes.FirstOrDefault().Value.Should().Be("test");
        }

        /// <summary>Verify object type is or is not a system type as expected.</summary>
        [Fact]
        public void Test_Type_IsSystemType()
        {
            // Arrange
            var testData = new TestClass { PropA = "test", PropB = "test" };

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

        /// <summary>Verify identity attribute can be identified.</summary>
        [Fact]
        public void Test_Type_HasIdentityAttribute()
        {
            // Arrange
            var testData = new IdentityExample()
            {
                Property1 = "test1",
                Property2 = "test2"
            };

            // Act
            var hasIdentity = testData.GetType().HasIdentityAttribute();
            var identityVal = testData.GetIdentityProperty();

            // Assert
            hasIdentity.Should().BeTrue();
            identityVal.Should().Be("test1");
        }

        /// <summary>Verify list of named attributes are returned.</summary>
        [Fact]
        public void Test_Type_HasNamedAttribute()
        {
            // Arrange
            var testData = new IdentityExample()
            {
                Property1 = "test1",
                Property2 = "test2"
            };

            // Act
            var identityProps = testData.GetType().GetPropertiesWithAttribute("Identity");

            // Assert
            identityProps.Count.Should().Be(1);
        }

        /// <summary>Verify sensitive data (SensitiveInfo and PersonalData) attributed properties are identified.</summary>
        [Fact]
        public void Test_Type_IsSensitiveData()
        {
            // Arrange
            var testData = new TestClass() {
                PropA = "test",
                PropB = "test",
                Name = "ROBERT",
                Dob = new DateTime(2000, 1, 1),
                PhoneNumber = "+442818283891",
                Password = "Password123"
            };

            // Act
            var hasPiiData = testData.GetType().HasPiiData();
            var piiDataFields = testData.GetType().GetPiiDataProperties();
            var hasSensitiveInfo = testData.GetType().HasSensitiveInfo();
            var sensitiveInfoFields = testData.GetType().GetSensitiveInfoProperties();
            var props = typeof(TestClass).Properties().ToList();

            // Assert
            hasPiiData.Should().BeTrue();
            hasSensitiveInfo.Should().BeTrue();
            piiDataFields.Keys.Contains("Dob").Should().BeTrue();
            piiDataFields.Keys.Contains("Name").Should().BeTrue();
            piiDataFields.Keys.Contains("PhoneNumber").Should().BeTrue();
            sensitiveInfoFields.Keys.Contains("Password").Should().BeTrue();

            props[0].PropertyType.Should().Be(testData.PropA.GetType());
            props[0].IsPiiData().Should().BeFalse();
            props[0].IsSensitiveInfo().Should().BeFalse();
            props[1].PropertyType.Should().Be(testData.PropB.GetType());
            props[1].IsPiiData().Should().BeFalse();
            props[1].IsSensitiveInfo().Should().BeFalse();
            props[6].PropertyType.Should().Be(testData.Name.GetType());
            props[6].IsPiiData().Should().BeTrue();
            props[6].IsSensitiveInfo().Should().BeFalse();
            props[7].PropertyType.Should().Be(testData.Dob.GetType());
            props[7].IsPiiData().Should().BeTrue();
            props[7].IsSensitiveInfo().Should().BeFalse();
            props[8].PropertyType.Should().Be(testData.PhoneNumber.GetType());
            props[8].IsPiiData().Should().BeTrue();
            props[8].IsSensitiveInfo().Should().BeFalse();
            props[9].PropertyType.Should().Be(testData.Password.GetType());
            props[9].IsSensitiveInfo().Should().BeTrue();
            props[9].IsPiiData().Should().BeFalse();
        }

        /// <summary>Verify a property can be read from an object using the property name.</summary>
        [Fact]
        public void Test_Object_GetPropertyValueByName()
        {
            // Arrange
            var testData = new NamedAttExample { Property1 = "test", Property2 = "test" };

            // Act
            var val = testData.GetPropertyValueByName("property1");
            var nonVal = testData.GetPropertyValueByName("doesntexist");

            // Assert
            val.Should().NotBeNull();
            val.Should().Be("test");
            nonVal.Should().BeNull();
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

            [PersonalData]
            public string Name { get; set; }

            [PersonalData]
            public DateTime Dob { get; set; }

            [Microsoft.AspNetCore.Identity.PersonalData]
            public string PhoneNumber { get; set; }

            [SensitiveInfo]
            public string Password { get; set; }
        }

        private class TestOther { }

        private class IdentityExample
        {
            [Identity]
            public string Property1 { get; set; }
            public string Property2 { get; set; }
        }

        private class NamedAttExample
        {
            [Named("RobTest1")]
            public string Property1 { get; set; }
            public string Property2 { get; set; }
        }
    }
}
