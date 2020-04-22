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
        public void Test_Stream_ConvertToFromString()
        {
            // Arrange - setup stream.
            string testText = "My Test String";
            Stream textStream = testText.ConvertToStream(Encoding.UTF8);
            byte[] stringBytes = Encoding.UTF8.GetBytes(testText);

            // Act - convert steam to bytes for comparison.
            byte[] streamBytes = textStream.CopyToBytes();

            // Assert - ensure both are equal, confirming method has worked.
            Assert.Equal(stringBytes, streamBytes);
        }
    }
}
