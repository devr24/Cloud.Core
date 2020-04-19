using System.IO;
using Cloud.Core.Tests.FakeObjects;
using Newtonsoft.Json;
using Cloud.Core.Testing;
using Xunit;

namespace Cloud.Core.Tests
{
    [IsUnit]
    public class JsonConverterTest
    {
        [Fact]
        public void Test_Deserialize_WithValue()
        {
            var jsonWithEnumValue = File.ReadAllText(@"fakeObjects\jsonObjectWithEnumValue.json");

            var testObject = JsonConvert.DeserializeObject<FakeObject>(jsonWithEnumValue);
            Assert.Equal("ObjectWithValue", testObject.Name);
            Assert.Equal(FakeEnum.Known1, testObject.FakeEnum);
        }

        [Fact]
        public void Test_Deserialize_WithEmptyValue()
        {
            var jsonWithEmptyEnumValue = File.ReadAllText(@"fakeObjects\jsonObjectWithEmptyEnumValue.json");

            var testObject = JsonConvert.DeserializeObject<FakeObject>(jsonWithEmptyEnumValue);
            Assert.Equal("ObjectWithEmptyValue", testObject.Name);
            Assert.Equal(FakeEnum.Default, testObject.FakeEnum);
        }

        [Fact]
        public void Test_Deserialize_WithNoValue()
        {
            var jsonWithNoEnumValue = File.ReadAllText(@"fakeObjects\jsonObjectWithNoEnumValue.json");

            var testObject = JsonConvert.DeserializeObject<FakeObject>(jsonWithNoEnumValue);
            Assert.Equal("ObjectWithNoValue", testObject.Name);
            Assert.Equal(FakeEnum.Default, testObject.FakeEnum);
        }

        [Fact]
        public void Test_Deserialize_WithInvalidValue()
        {
            var jsonWithInvalidEnumValue = File.ReadAllText(@"fakeObjects\jsonObjectWithInvalidEnumValue.json");

            var testObject = JsonConvert.DeserializeObject<FakeObject>(jsonWithInvalidEnumValue);
            Assert.Equal("ObjectWithInvalidValue", testObject.Name);
            Assert.Equal(FakeEnum.Default, testObject.FakeEnum);
        }

        [Fact]
        public void Test_Serialize_WithValue()
        {
            var testObject = new FakeObject
            {
                FakeEnum = FakeEnum.Known2,
                Name = "ObjectWithValue"
            };

            var jsonString = JsonConvert.SerializeObject(testObject);

            Assert.True(!string.IsNullOrWhiteSpace(jsonString));

            var deserializedObject = JsonConvert.DeserializeObject<FakeObject>(jsonString);
            Assert.Equal("ObjectWithValue", testObject.Name);
            Assert.Equal(FakeEnum.Known2, testObject.FakeEnum);
        }

        [Fact]
        public void Test_Serialize_WithNoValue()
        {
            var testObject = new FakeObject
            {
                Name = "ObjectWithDefaultValue"
            };

            var jsonString = JsonConvert.SerializeObject(testObject);

            Assert.True(!string.IsNullOrWhiteSpace(jsonString));

            var deserializedObject = JsonConvert.DeserializeObject<FakeObject>(jsonString);
            Assert.Equal("ObjectWithDefaultValue", testObject.Name);
            Assert.Equal(FakeEnum.Default, testObject.FakeEnum);
        }
    }
}
