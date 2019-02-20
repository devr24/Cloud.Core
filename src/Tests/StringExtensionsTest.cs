using System;
using System.IO;
using System.Text;
using Cloud.Core.Testing;
using Xunit;
using FluentAssertions;

namespace Cloud.Core.Tests
{
    public class StringExtensionsTest
    {
        [Theory, IsUnit]
        [InlineData("This is a test string")]
        public void Test_GetSizeInBytes_Positive(string value)
        {
            // Arrange and act.
            var streamLen = value.ConvertToStream(Encoding.UTF8).Length;
            var size = value.GetSizeInBytes(Encoding.UTF8);

            // Assert.
            size.Should().Be(streamLen);
        }

        [Theory, IsUnit]
        [InlineData(null)]
        [InlineData("")]
        public void Test_IsNullOrEmpty_Positive(string value)
        {
            Assert.True(value.IsNullOrEmpty());
        }

        [Theory, IsUnit]
        [InlineData("TEST")]
        [InlineData(" ")]
        public void Test_IsNullOrEmpty_Negative(string value)
        {
            Assert.False(value.IsNullOrEmpty());
        }

        [Theory, IsUnit]
        [InlineData("TEST")]
        [InlineData(" TEST ")]
        public void Test_StreamConversion(string value)
        {
            MemoryStream toStream = value.ConvertToStream(Encoding.UTF8);
            var fromStream = toStream.ConvertToString();
            Assert.Equal(value, fromStream);
        }

        [Theory, IsUnit]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Test_IsNullOrWhiteSpace_Positive(string value)
        {
            Assert.True(value.IsNullOrWhiteSpace());
        }

        [Theory, IsUnit]
        [InlineData("TEST")]
        [InlineData(" TEST ")]
        public void Test_IsNullOrWhiteSpace_Negative(string value)
        {
            Assert.False(value.IsNullOrWhiteSpace());
        }

        [Theory, IsUnit]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Test_ThrowIfNullOrWhiteSpace(string value)
        {
            var ex = Assert.Throws<ArgumentNullException>(() => value.ThrowIfNullOrWhiteSpace());
            Assert.Equal("Value cannot be null.\r\nParameter name: value", ex.Message);
        }

        [Fact, IsUnit]
        public void Test_AddSpaceBeforeCaps_Split()
        {
            string value = "thisTestString";
            string expected = "this Test String";

            Assert.Equal(value.AddSpaceBeforeCaps(), expected);
        }

        [Fact, IsUnit]
        public void Test_AddSpaceBeforeCaps_Null()
        {
            string value = null;
            Assert.Null(value.AddSpaceBeforeCaps());
        }

        [Theory, IsUnit]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("thisisnotaguid")]
        public void Test_ToGuid_Negative(string value)
        {
            var result = value.ToGuid();

            Assert.Equal(Guid.Empty, result);
        }

        [Theory, IsUnit]
        [InlineData("E540A2B7-D1B3-4770-B7FE-E435DBBC9D64")]
        public void Test_ToGuid_Positive(string value)
        {
            var result = value.ToGuid();
            var expected = new Guid(value);
            Assert.Equal(expected, result);
        }

        [Theory, IsUnit]
        [InlineData("TEST1", "TEST1")]
        [InlineData("TEST2", "TEST2")]
        public void Test_IsEquivalentTo_Positive(string value, string compare)
        {
            Assert.True(value.IsEquivalentTo(compare));
        }

        [Theory, IsUnit]
        [InlineData("TEST1", "TEST2")]
        [InlineData("TEST2", "TEST3")]
        public void Test_IsEquivalentTo_Negative(string value, string compare)
        {
            Assert.False(value.IsEquivalentTo(compare));
        }

        [Theory, IsUnit]
        [InlineData("1", 1)]
        [InlineData("2", 2)]
        public void Test_ToInt32_Positive(string value, Int32 expected)
        {
            Assert.Equal(value.ToInt32(), expected);
        }

        [Theory, IsUnit]
        [InlineData("")]
        [InlineData("TEST")]
        public void Test_ToInt32_Negative(string value)
        {
            Assert.ThrowsAny<Exception>(() => value.ToInt32());
        }

        [Theory, IsUnit]
        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("True", true)]
        [InlineData("False", false)]
        [InlineData("1", true)]
        [InlineData("0", false)]
        [InlineData("T", true)]
        [InlineData("F", false)]
        [InlineData("t", true)]
        [InlineData("f", false)]
        [InlineData("Y", true)]
        [InlineData("N", false)]
        [InlineData("anyOther", false)]
        public void Test_ToBoolean_Positive(string value, bool expected)
        {
            Assert.Equal(value.ToBoolean(), expected);
        }

        [Theory, IsUnit]
        [InlineData("")]
        [InlineData(" ")]
        public void Test_ToBoolean_Negative(string value)
        {
            Assert.ThrowsAny<Exception>(() => value.ToBoolean());
        }

        [Theory, IsUnit]
        [InlineData("anyOther", "a")]
        [InlineData("anyOther", "any")]
        [InlineData("anyOther", "other")]
        [InlineData("anyOther", "Other")]
        public void Test_ContainsEquivalent_Positive(string value, string comparer)
        {
            Assert.True(value.ContainsEquivalent(comparer));
        }

        [Theory, IsUnit]
        [InlineData("")]
        [InlineData(" ")]
        public void Test_ContainsEquivalent_Negative(string value)
        {
            Assert.ThrowsAny<Exception>(() => value.ContainsEquivalent(null));
        }

        [Theory, IsUnit]
        [InlineData("anyOther", "a")]
        [InlineData("anyOther", "any")]
        [InlineData("myTest", "m")]
        [InlineData("myTest", "myte")]
        public void Test_StartsWithEquivalent_Positive(string value, string comparer)
        {
            Assert.True(value.StartsWithEquivalent(comparer));
        }

        [Theory, IsUnit]
        [InlineData("")]
        [InlineData(" ")]
        public void Test_StartsWithEquivalent_Negative(string value)
        {
            Assert.ThrowsAny<Exception>(() => value.StartsWithEquivalent(null));
        }

        [Theory, IsUnit]
        [InlineData("anyOther", "r")]
        [InlineData("anyOther", "er")]
        [InlineData("myTest", "est")]
        [InlineData("myTest", "test")]
        public void Test_EndsWithEquivalent_Positive(string value, string comparer)
        {
            Assert.True(value.EndsWithEquivalent(comparer));
        }

        [Theory, IsUnit]
        [InlineData("")]
        [InlineData(" ")]
        public void Test_EndsWithEquivalent_Negative(string value)
        {
            Assert.ThrowsAny<Exception>(() => value.EndsWithEquivalent(null));
        }
    }
}
