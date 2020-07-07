using System.IO;
using Cloud.Core.Tests.FakeObjects;
using Newtonsoft.Json;
using Cloud.Core.Testing;
using FluentAssertions;
using Xunit;

namespace Cloud.Core.Tests
{
    [IsUnit]
    public class JsonGenericEnumStringConverterTest
    {
        /// <summary>Verify enum value is correctly deserialized as expected.</summary>
        [Fact]
        public void Test_Deserialize_WithValue()
        {
            // Arrange
            var jsonWithEnumValue = File.ReadAllText(@"fakeObjects\jsonObjectWithEnumValue.json");

            // Act
            var testObject = JsonConvert.DeserializeObject<FakeObject>(jsonWithEnumValue);

            // Assert
            Assert.Equal("ObjectWithValue", testObject.Name);
            Assert.Equal(FakeEnum.Known1, testObject.FakeEnum);
        }

        /// <summary>Verify emtpy value is deserialized as expected.</summary>
        [Fact]
        public void Test_Deserialize_WithEmptyValue()
        {
            // Arrange
            var jsonWithEmptyEnumValue = File.ReadAllText(@"fakeObjects\jsonObjectWithEmptyEnumValue.json");

            // Act
            var testObject = JsonConvert.DeserializeObject<FakeObject>(jsonWithEmptyEnumValue);

            // Assert
            Assert.Equal("ObjectWithEmptyValue", testObject.Name);
            Assert.Equal(FakeEnum.Default, testObject.FakeEnum);
        }

        /// <summary>Verify deserialized as expected when no value is specified.</summary>
        [Fact]
        public void Test_Deserialize_WithNoValue()
        {
            // Arrange
            var jsonWithNoEnumValue = File.ReadAllText(@"fakeObjects\jsonObjectWithNoEnumValue.json");

            // Act
            var testObject = JsonConvert.DeserializeObject<FakeObject>(jsonWithNoEnumValue);

            // Assert
            Assert.Equal("ObjectWithNoValue", testObject.Name);
            Assert.Equal(FakeEnum.Default, testObject.FakeEnum);
        }

        /// <summary>Verify deserialized as expected when invalid value is specified.</summary>
        [Fact]
        public void Test_Deserialize_WithInvalidValue()
        {
            // Arrange
            var jsonWithInvalidEnumValue = File.ReadAllText(@"fakeObjects\jsonObjectWithInvalidEnumValue.json");

            // Act
            var testObject = JsonConvert.DeserializeObject<FakeObject>(jsonWithInvalidEnumValue);

            // Assert
            Assert.Equal("ObjectWithInvalidValue", testObject.Name);
            Assert.Equal(FakeEnum.Default, testObject.FakeEnum);
        }

        /// <summary>Verify enum value is correctly serialized and subsequently deserialized as expected.</summary>
        [Fact]
        public void Test_Serialize_WithValue()
        {
            // Arrange
            var testObject = new FakeObject {
                FakeEnum = FakeEnum.Known2,
                Name = "ObjectWithValue"
            };

            // Act
            var jsonString = JsonConvert.SerializeObject(testObject);
            var deserializedObject = JsonConvert.DeserializeObject<FakeObject>(jsonString);

            // Assert
            Assert.True(!string.IsNullOrWhiteSpace(jsonString));
            Assert.Equal("ObjectWithValue", testObject.Name);
            Assert.Equal(FakeEnum.Known2, testObject.FakeEnum);
            testObject.Should().BeEquivalentTo(deserializedObject);
        }

        /// <summary>Verify that when no value is specified, that serializing and deserializing works as expected.</summary>
        [Fact]
        public void Test_Serialize_WithNoValue()
        {
            // Arrange
            var testObject = new FakeObject {
                Name = "ObjectWithDefaultValue"
            };

            // Act
            var jsonString = JsonConvert.SerializeObject(testObject);
            var deserializedObject = JsonConvert.DeserializeObject<FakeObject>(jsonString);

            // Assert
            Assert.True(!string.IsNullOrWhiteSpace(jsonString));
            Assert.Equal("ObjectWithDefaultValue", testObject.Name);
            Assert.Equal(FakeEnum.Default, testObject.FakeEnum);
            testObject.Should().BeEquivalentTo(deserializedObject);
        }

        /// <summary>Verifies conversion happens as expected.</summary>
        [Fact]
        public void Test_JsonConvertExtension_TryDesrialize_Success()
        {
            // Arrange
            var example = new ExampleModel { 
                Prop1 = "a",
                Prop2 = "b"
            };

            // Act
            var serializedVersion = JsonConvert.SerializeObject(example);
            var result = JsonConvertExtensions.TryDeserialize<ExampleModel>(serializedVersion);

            // Assert
            result.Should().NotBeNull();
            result.Prop1.Should().Be(example.Prop1);
            result.Prop2.Should().Be(example.Prop2);
        }

        /// <summary>Verifies conversion fails as expected but no exception is thrown.</summary>
        [Fact]
        public void Test_JsonConvertExtension_TryDesrialize_Failure()
        {
            // Arrange
            var notDeserializable = "asdasdasdasd";

            // Act
            var result = JsonConvertExtensions.TryDeserialize<ExampleModel>(notDeserializable);

            // Assert
            result.Should().BeNull();
        }

        private class ExampleModel
        {
            public string Prop1 { get; set; }
            public string Prop2 { get; set; }
        }
    }
}
