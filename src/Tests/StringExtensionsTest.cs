using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cloud.Core.Extensions;
using Cloud.Core.Testing;
using FluentAssertions;
using Xunit;

namespace Cloud.Core.Tests
{
    [IsUnit]
    public class StringExtensionsTest
    {
        /// <summary>Verify content between placeholders is found as expected.</summary>
        [Theory]
        [InlineData("{{", "}}", "OBJECT1,OBJECT2,OBJECT3")]
        [InlineData("<<", ">>", "OBJECT4")]
        [InlineData("mollit", "est", " anim id ")]
        [InlineData("[[", ">>", "OBJECT5")]
        public void Test_String_FindBetweenDelimiters(string startDelimiter, string endDelimiter, string expected)
        {
            // Arrange
            var expectedResult = expected.Split(",").ToHashSet();
            var searchString = "Lorem ipsum dolor sit amet, {{OBJECT1}} adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, " +
                "quis nostrud exercitation {{OBJECT2}} laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore " +
                "eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa {{OBJECT3}} officia deserunt mollit anim id est <<OBJECT4>>. Duis aute irure dolor in reprehenderit in voluptate [[OBJECT5>>";

            // Act
            var results = searchString.FindBetweenDelimiters(startDelimiter, endDelimiter);

            // Assert
            results.Should().BeEquivalentTo(expectedResult);
            results.Count.Should().Be(expectedResult.Count);
        }

        /// <summary>Verify content between placeholders is found and substituted using a model expected.</summary>
        [Fact]
        public void Test_String_SubstitutePlaceholders()
        {
            // Arrange
            var searchString = "Lorem ipsum dolor sit amet, {{OBJECT1}} adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, " +
                               "quis nostrud exercitation {{OBJECT2}} laboris nisi ut aliquip ex ea commodo, sunt in culpa {{OBJECT3}} officia deserunt mollit anim id est <<OBJECT4>>. " +
                               "Duis aute irure dolor in reprehenderit in voluptate [[OBJECT5>>";

            var expectedResult = "Lorem ipsum dolor sit amet, ROB adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, " +
                                 "quis nostrud exercitation ROB laboris nisi ut aliquip ex ea commodo, sunt in culpa {{OBJECT3}} officia deserunt mollit anim id est <<OBJECT4>>. " +
                                 "Duis aute irure dolor in reprehenderit in voluptate [[OBJECT5>>";

            // Act
            var model = new { Object1 = "ROB", Object2 = "ROB" };
            var result = searchString.SubstitutePlaceholders(model, "{{", "}}");
            var keys = string.Join(",", result.PlaceholderKeys);

            // Assert
            result.SubstitutedContent.Should().BeEquivalentTo(expectedResult);
            result.ModelKeyValues.Should().BeEquivalentTo(new Dictionary<string, string>
            {
                { "object1", "ROB" },
                { "object2", "ROB" }
            });
            result.SubstitutedValueCount.Should().Be(2);
            keys.Should().BeEquivalentTo("OBJECT1,OBJECT2,OBJECT3");
        }

        /// <summary>Ensure non-alphanumeric characters are removed from the given string.</summary>
        [Fact]
        public void Test_String_CleanContent()
        {
            // Arrange
            var source = $"This\n is  {Environment.NewLine}   a   cleaned string ! - £$$%(*71 ";

            // Act
            var replaced = source.RemoveNonAlphanumericCharacters();

            // Assert
            replaced.Should().Be("This is a cleaned string 71 ");
        }

        /// <summary>Ensure multiple matches of a specified string are removed as expected.</summary>
        [Fact]
        public void Test_String_RemoveMultiple()
        {
            // Arrange
            var source = "my test string is here";

            // Act
            var replaced = source.RemoveMultiple("is", "test");

            // Assert
            replaced.Should().Be("my  string  here");
        }

        /// <summary>Ensure multiple matches of a specified string are removed as expected and non matches are ignored.</summary>
        [Fact]
        public void Test_String_ReplaceMultiple()
        {
            // Arrange
            var source = "my test string is here";

            // Act
            var replaced = source.ReplaceMultiple("text", "is", "test");

            // Assert
            replaced.Should().Be("my text string text here");
        }

        /// <summary>Ensure default if null or emtype sets the default value as expected.</summary>
        [Fact]
        public void Test_String_DefaultIfNullOrEmtpy()
        {
            // Arrange
            var original = "";

            // Act
            original = original.DefaultIfNullOrEmtpy("default");

            // Assert
            original.Should().Be("default");
        }

        /// <summary>Ensure multiple lines are created as expected.</summary>
        [Fact]
        public void Test_String_MultiLine()
        {
            // Arrange
            var multi = StringExtensions.MultiLine("lineone","linetwo");

            // Act
            int numLines = multi.Split('\n').Length;

            // Assert
            numLines.Should().Be(2);
        }

        /// <summary>Ensure substring by removing start and end results in expected.</summary>
        [Fact]
        public void Test_String_SubstringRemveStartAndEnd()
        {
            // Arrange
            var test = "MyTest,String.IsThis";

            // Act
            var result = test.Substring("MyTest,", ".IsThis");

            // Assert
            result.Should().Be("String");
            result = test.Substring("Test,", ".Is");
            result.Should().Be("String");
        }

        /// <summary>Ensure SetStringIfNullOrEmpty results in result expected.</summary>
        [Fact]
        public void Test_String_SetDefaultIfNullOrEmpty()
        {
            // Assert does not set default.
            "start".SetDefaultIfNullOrEmpty("default").Should().Be("start");

            // Assert null string defaults.
            ((string)null).SetDefaultIfNullOrEmpty("default").Should().Be("default");

            // Assert empty string defaults.
            "".SetDefaultIfNullOrEmpty("default").Should().Be("default");
        }

        /// <summary>Ensure characters are replaced as expected.</summary>
        [Theory]
        [InlineData("This is a test string", new [] { ' ' }, "Thisisateststring")]
        [InlineData("This,is;another|test string", new [] { ' ', ',', ';', '|' }, "Thisisanotherteststring")]
        public void Test_String_ReplaceChars(string value, char[] replaceChars, string expectedResult)
        {
            // Arrange and act.
            var result = value.ReplaceAll(replaceChars, string.Empty);

            // Assert.
            result.Should().Be(expectedResult);
        }

        /// <summary>Ensure size bytes results in expected.</summary>
        [Theory]
        [InlineData("This is a test string")]
        public void Test_String_GetSizeInBytes(string value)
        {
            // Arrange and act.
            var streamLen = value.ConvertToStream(Encoding.UTF8).Length;
            var size = value.GetSizeInBytes(Encoding.UTF8);

            // Assert.
            size.Should().Be(streamLen);
        }

        /// <summary>Ensure is null or empty gives the correct result as expected when null or empty is used.</summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Test_String_IsNullOrEmpty_Positive(string value)
        {
            Assert.True(value.IsNullOrEmpty());
        }

        /// <summary>Ensure is null or empty gives the correct result as expected when string is set.</summary>
        [Theory]
        [InlineData("TEST")]
        [InlineData(" ")]
        public void Test_String_IsNullOrEmpty_Negative(string value)
        {
            Assert.False(value.IsNullOrEmpty());
        }

        /// <summary>Ensure string converts to and from stream successfully.</summary>
        [Theory]
        [InlineData("TEST")]
        [InlineData(" TEST ")]
        public void Test_String_ToFromStreamConversion(string value)
        {
            // Arrange/Act
            MemoryStream toStream = value.ConvertToStream(Encoding.UTF8);
            var fromStream = toStream.ReadContents();

            // Assert
            Assert.Equal(value, fromStream);
        }

        /// <summary>Ensure is null or white space gives expected true result.</summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Test_String_IsNullOrWhiteSpace_Positive(string value)
        {
            // Act/Assert
            Assert.True(value.IsNullOrWhiteSpace());
        }

        /// <summary>Ensure is null or white space gives expected false result.</summary>
        [Theory]
        [InlineData("TEST")]
        [InlineData(" TEST ")]
        public void Test_String_IsNullOrWhiteSpace_Negative(string value)
        {
            // Act/Assert
            Assert.False(value.IsNullOrWhiteSpace());
        }

        /// <summary>Ensure exception is thrown if null or whitespace.</summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Test_String_ThrowIfNullOrWhiteSpace(string value)
        {
            // Act/Assert
            Assert.Throws<ArgumentNullException>(() => value.ThrowIfNullOrWhiteSpace());
        }

        /// <summary>Ensure space is added before each capital letter.</summary>
        [Fact]
        public void Test_String_AddSpaceBeforeCaps()
        {
            // Arrange
            string value = "thisTestString";
            string expected = "this Test String";

            // Act/Assert
            Assert.Equal(value.AddSpaceBeforeCaps(), expected);
        }

        /// <summary>Ensure null is returned when null string requests AddSpaceBefore caps.</summary>
        [Fact]
        public void Test_AddSpaceBeforeCaps_Null()
        {
            // Arrange
            string value = null;

            // Act/Assert
            Assert.Null(value.AddSpaceBeforeCaps());
        }

        /// <summary>Ensure invalid guid strings are returned as an empty guid.</summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("thisisnotaguid")]
        public void Test_String_ToEmptyGuid(string value)
        {
            // Arrange/Act
            var result = value.ToGuid();

            // Assert
            Assert.Equal(Guid.Empty, result);
        }

        /// <summary>Ensure valid guid strings are returned as an expected guid.</summary>
        [Theory]
        [InlineData("E540A2B7-D1B3-4770-B7FE-E435DBBC9D64")]
        public void Test_String_ToGuid(string value)
        {
            // Arrange/Act
            var result = value.ToGuid();
            var expected = new Guid(value);

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>Ensure comparison returns true when two string objects are compared for equivilence.</summary>
        [Theory]
        [InlineData("TEST1", "TEST1")]
        [InlineData("TEST2", "TEST2")]
        public void Test_IsEquivalentTo_Positive(string value, string compare)
        {
            // Act/Assert
            Assert.True(value.IsEquivalentTo(compare));
        }

        /// <summary>Ensure comparison returns false when two string objects are compared for equivilence.</summary>
        [Theory]
        [InlineData("TEST1", "TEST2")]
        [InlineData("TEST2", "TEST3")]
        public void Test_IsEquivalentTo_Negative(string value, string compare)
        {
            // Act/Assert
            Assert.False(value.IsEquivalentTo(compare));
        }

        /// <summary>Ensure string to int conversion works as expected.</summary>
        [Theory]
        [InlineData("1", 1)]
        [InlineData("2", 2)]
        public void Test_StringToInt32(string value, int expected)
        {
            // Act/Assert
            Assert.Equal(value.ToInt32(), expected);
        }

        /// <summary>Ensure string to int conversion throw exception when the string cannot be converted.</summary>
        [Theory]
        [InlineData("")]
        [InlineData("TEST")]
        public void Test_String_ToInt32WithException(string value)
        {
            // Act/Assert
            Assert.ThrowsAny<Exception>(() => value.ToInt32());
        }

        /// <summary>Ensure string to boolean conversion works as expected.</summary>
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
        public void Test_String_ToBoolean(string value, bool expected)
        {
            // Act/Assert
            Assert.Equal(value.ToBoolean(), expected);
        }

        /// <summary>Ensure string to boolean conversion throws exception when string is empty or null.</summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Test_String_ToBooleanWithException(string value)
        {
            // Act/Assert
            Assert.ThrowsAny<Exception>(() => value.ToBoolean());
        }

        /// <summary>Ensure string contains equivalent returns true.</summary>
        [Theory]
        [InlineData("anyOther", "a")]
        [InlineData("anyOther", "any")]
        [InlineData("anyOther", "other")]
        [InlineData("anyOther", "Other")]
        public void Test_String_ContainsEquivalent(string value, string comparer)
        {
            // Act/Assert
            Assert.True(value.ContainsEquivalent(comparer));
        }

        /// <summary>Ensure string contains equivalent throws exception when empty is passed.</summary>
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Test_ContainsEquivalentWithException(string value)
        {
            // Act/Assert
            Assert.ThrowsAny<Exception>(() => value.ContainsEquivalent(null));
        }

        /// <summary>Ensure string starts with equivalent returns true when matched.</summary>
        [Theory]
        [InlineData("anyOther", "a")]
        [InlineData("anyOther", "any")]
        [InlineData("myTest", "m")]
        [InlineData("myTest", "myte")]
        public void Test_StartsWithEquivalent(string value, string comparer)
        {
            // Act/Assert
            Assert.True(value.StartsWithEquivalent(comparer));
        }

        /// <summary>Ensure string starts with equivalent throws exception when empty is passed.</summary>
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Test_StartsWithEquivalentWithException(string value)
        {
            // Act/Assert
            Assert.ThrowsAny<Exception>(() => value.StartsWithEquivalent(null));
        }

        /// <summary>Ensure string ends with equivalent returns true when end of string is matched.</summary>
        [Theory]
        [InlineData("anyOther", "r")]
        [InlineData("anyOther", "er")]
        [InlineData("myTest", "est")]
        [InlineData("myTest", "test")]
        public void Test_EndsWithEquivalent(string value, string comparer)
        {
            // Act/Assert
            Assert.True(value.EndsWithEquivalent(comparer));
        }

        /// <summary>Ensure string ends with equivalent throws exception when or empty is passed.</summary>
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Test_EndsWithEquivalentWithException(string value)
        {
            // Act/Assert
            Assert.ThrowsAny<Exception>(() => value.EndsWithEquivalent(null));
        }
    }
}
