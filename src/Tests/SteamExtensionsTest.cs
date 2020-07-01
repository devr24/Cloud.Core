using System;
using System.IO;
using System.Text;
using Cloud.Core.Testing;
using Xunit;

namespace Cloud.Core.Tests
{
    [IsUnit]
    public class StreamExtensionsTest
    {
        /// <summary>Check the string is converted into stream and back into string as expected.</summary>
        [Fact]
        public async void Test_Stream_ConvertToFromString()
        {
            // Arrange - setup stream.
            string testText = "My Test String";
            Stream textStream = testText.ConvertToStream(Encoding.UTF8);
            byte[] stringBytes = Encoding.UTF8.GetBytes(testText);

            // Act - convert steam to bytes for comparison.
            byte[] streamBytes = await textStream.ToBytes();

            // Assert - ensure both are equal, confirming method has worked.
            Assert.Equal(stringBytes, streamBytes);
        }

        /// <summary>Convert to base64 and back again.</summary>
        [Theory]
        [InlineData("test data text")]
        [InlineData("other test data")]
        public void Test_Stream_ConvertToFromBase64(string testString)
        {
            // Arrange - setup stream.
            Stream textStream = testString.ConvertToStream(Encoding.UTF8);

            // Act - convert steam to bytes for comparison.
            var base64 = textStream.ToBase64().GetAwaiter().GetResult();
            var bytes = textStream.ToBytes().GetAwaiter().GetResult();
            var revert = Convert.FromBase64String(base64);

            // Assert - ensure both are equal, confirming method has worked.
            Assert.Equal(bytes, revert);
        }
    }
}
