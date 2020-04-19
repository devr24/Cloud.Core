using System;
using System.IO;
using System.Text;
using Cloud.Core.Testing;
using Xunit;
using FluentAssertions;

namespace Cloud.Core.Tests
{
    [IsUnit]
    public class StringExtensionsTest
    {
        [Fact]
        public void Test_CleanContent()
        {
            var source = $"This\n is  {Environment.NewLine}   a   cleaned string ! - £$$%(*71 ";
            var replaced = source.RemoveNonAlphanumericCharacters();
            replaced.Should().Be("This is a cleaned string 71 ");
        }

        [Fact]
        public void Test_RemoveMultiple()
        {
            var source = "my test string is here";
            var replaced = source.RemoveMultiple("is", "test");
            replaced.Should().Be("my  string  here");
        }

        [Fact]
        public void Test_ReplaceMultiple()
        {
            var source = "my test string is here";
            var replaced = source.ReplaceMultiple("text", "is", "test");
            replaced.Should().Be("my text string text here");
        }

        [Fact]
        public void Test_MultiLine()
        {
            var multi = StringExtensions.MultiLine("lineone","linetwo");
            int numLines = multi.Split('\n').Length;
            numLines.Should().Be(2);
        }

        [Fact]
        public void Test_Substring()
        {
            var test = "MyTest,String.IsThis";

            var result = test.Substring("MyTest,", ".IsThis");

            result.Should().Be("String");

            result = test.Substring("Test,", ".Is");

            result.Should().Be("String");
        }

        [Fact]
        public void Test_SetDefaultIfNullOrEmpty()
        {
            "start".SetDefaultIfNullOrEmpty("default").Should().Be("start");
            ((string)null).SetDefaultIfNullOrEmpty("default").Should().Be("default");
            "".SetDefaultIfNullOrEmpty("default").Should().Be("default");
        }

        [Theory]
        [InlineData("This is a test string", new [] { ' ' }, "Thisisateststring")]
        [InlineData("This,is;another|test string", new [] { ' ', ',', ';', '|' }, "Thisisanotherteststring")]
        public void Test_ReplaceChars(string value, char[] replaceChars, string expectedResult)
        {
            // Arrange and act.
            var result = value.ReplaceAll(replaceChars, string.Empty);

            // Assert.
            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("This is a test string")]
        public void Test_GetSizeInBytes_Positive(string value)
        {
            // Arrange and act.
            var streamLen = value.ConvertToStream(Encoding.UTF8).Length;
            var size = value.GetSizeInBytes(Encoding.UTF8);

            // Assert.
            size.Should().Be(streamLen);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Test_IsNullOrEmpty_Positive(string value)
        {
            Assert.True(value.IsNullOrEmpty());
        }

        [Theory]
        [InlineData("TEST")]
        [InlineData(" ")]
        public void Test_IsNullOrEmpty_Negative(string value)
        {
            Assert.False(value.IsNullOrEmpty());
        }

        [Theory]
        [InlineData("TEST")]
        [InlineData(" TEST ")]
        public void Test_StreamConversion(string value)
        {
            MemoryStream toStream = value.ConvertToStream(Encoding.UTF8);
            var fromStream = toStream.ConvertToString();
            Assert.Equal(value, fromStream);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Test_IsNullOrWhiteSpace_Positive(string value)
        {
            Assert.True(value.IsNullOrWhiteSpace());
        }

        [Theory]
        [InlineData("TEST")]
        [InlineData(" TEST ")]
        public void Test_IsNullOrWhiteSpace_Negative(string value)
        {
            Assert.False(value.IsNullOrWhiteSpace());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Test_ThrowIfNullOrWhiteSpace(string value)
        {
            Assert.Throws<ArgumentNullException>(() => value.ThrowIfNullOrWhiteSpace());
        }

        [Fact]
        public void Test_AddSpaceBeforeCaps_Split()
        {
            string value = "thisTestString";
            string expected = "this Test String";

            Assert.Equal(value.AddSpaceBeforeCaps(), expected);
        }

        [Fact]
        public void Test_AddSpaceBeforeCaps_Null()
        {
            string value = null;
            Assert.Null(value.AddSpaceBeforeCaps());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("thisisnotaguid")]
        public void Test_ToGuid_Negative(string value)
        {
            var result = value.ToGuid();

            Assert.Equal(Guid.Empty, result);
        }

        [Theory]
        [InlineData("E540A2B7-D1B3-4770-B7FE-E435DBBC9D64")]
        public void Test_ToGuid_Positive(string value)
        {
            var result = value.ToGuid();
            var expected = new Guid(value);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("TEST1", "TEST1")]
        [InlineData("TEST2", "TEST2")]
        public void Test_IsEquivalentTo_Positive(string value, string compare)
        {
            Assert.True(value.IsEquivalentTo(compare));
        }

        [Theory]
        [InlineData("TEST1", "TEST2")]
        [InlineData("TEST2", "TEST3")]
        public void Test_IsEquivalentTo_Negative(string value, string compare)
        {
            Assert.False(value.IsEquivalentTo(compare));
        }

        [Theory]
        [InlineData("1", 1)]
        [InlineData("2", 2)]
        public void Test_ToInt32_Positive(string value, Int32 expected)
        {
            Assert.Equal(value.ToInt32(), expected);
        }

        [Theory]
        [InlineData("")]
        [InlineData("TEST")]
        public void Test_ToInt32_Negative(string value)
        {
            Assert.ThrowsAny<Exception>(() => value.ToInt32());
        }

        [Theory]
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

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Test_ToBoolean_Negative(string value)
        {
            Assert.ThrowsAny<Exception>(() => value.ToBoolean());
        }

        [Theory]
        [InlineData("anyOther", "a")]
        [InlineData("anyOther", "any")]
        [InlineData("anyOther", "other")]
        [InlineData("anyOther", "Other")]
        public void Test_ContainsEquivalent_Positive(string value, string comparer)
        {
            Assert.True(value.ContainsEquivalent(comparer));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Test_ContainsEquivalent_Negative(string value)
        {
            Assert.ThrowsAny<Exception>(() => value.ContainsEquivalent(null));
        }

        [Theory]
        [InlineData("anyOther", "a")]
        [InlineData("anyOther", "any")]
        [InlineData("myTest", "m")]
        [InlineData("myTest", "myte")]
        public void Test_StartsWithEquivalent_Positive(string value, string comparer)
        {
            Assert.True(value.StartsWithEquivalent(comparer));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Test_StartsWithEquivalent_Negative(string value)
        {
            Assert.ThrowsAny<Exception>(() => value.StartsWithEquivalent(null));
        }

        [Theory]
        [InlineData("anyOther", "r")]
        [InlineData("anyOther", "er")]
        [InlineData("myTest", "est")]
        [InlineData("myTest", "test")]
        public void Test_EndsWithEquivalent_Positive(string value, string comparer)
        {
            Assert.True(value.EndsWithEquivalent(comparer));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Test_EndsWithEquivalent_Negative(string value)
        {
            Assert.ThrowsAny<Exception>(() => value.EndsWithEquivalent(null));
        }
    }
}
