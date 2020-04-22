using System;
using Cloud.Core.Testing;
using Xunit;

namespace Cloud.Core.Tests
{
    [IsUnit]
    public class IntExtensionsTest
    {
        /// <summary>Ensure the size suffix throws an exception as expected.</summary>
        [Fact]
        public void Test_SizeSuffix_Throws()
        {
            // Arrange
            long size = 0;

            // Act/Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => size.ToSizeSuffix(-1));
        }

        /// <summary>Ensure the size suffix converts to negative bytes string label as expected.</summary>
        [Fact]
        public void Test_SizeSuffix_AsMinusBytes()
        {
            // Arrange
            long size = -10;

            // Act/Assert
            Assert.Equal("-10.0 bytes",size.ToSizeSuffix());
        }

        /// <summary>Ensure the size suffix converts to zero bytes string label as expected.</summary>
        [Fact]
        public void Test_SizeSuffix_AsBytes()
        {
            // Arrange
            long size = 0;

            // Act/Assert
            Assert.Equal("0.0 bytes", size.ToSizeSuffix());
        }

        /// <summary>Ensure the size suffix converts to bytes with decimals string label as expected.</summary>
        [Fact]
        public void Test_SizeSuffix_AsBytesWithDecimal()
        {
            // Arrange
            long size = 0;

            // Act/Assert
            Assert.Equal("0.00000 bytes", size.ToSizeSuffix(5));
        }

        /// <summary>Ensure the size suffix converts to kilobytes string label as expected.</summary>
        [Fact]
        public void Test_SizeSuffix_AsKilobytes()
        {
            // Arrange
            long size = 1024;

            // Act/Assert
            Assert.Equal("1.0 KB", size.ToSizeSuffix());
        }

        /// <summary>Ensure the size suffix converts to kilobytes with decimal string label as expected.</summary>
        [Fact]
        public void Test_SizeSuffix_AsKilobytesWithDecimal()
        {
            // Arrange
            long size = 1024;

            // Act/Assert
            Assert.Equal("1.00000 KB", size.ToSizeSuffix(5));
        }

        /// <summary>Ensure the size suffix converts to megabytes string label as expected.</summary>
        [Fact]
        public void Test_SizeSuffix_AsMegabytes()
        {
            // Arrange
            long size = 1024 * 1024;

            // Act/Assert
            Assert.Equal("1.0 MB", size.ToSizeSuffix());
        }

        /// <summary>Ensure the size suffix converts to gigabytes string label as expected.</summary>
        [Fact]
        public void Test_SizeSuffix_AsGigabytes()
        {
            // Arrange
            long size = 1048576 * 1024;

            // Act/Assert
            Assert.Equal("1.0 GB", size.ToSizeSuffix());
        }

        /// <summary>Ensure the size suffix converts to terrabytes string label as expected.</summary>
        [Fact]
        public void Test_SizeSuffix_AsTerabytes()
        {
            // Arrange
            long size = 1048576 * 1024;
            size *= 1024;

            // Act/Assert
            Assert.Equal("1.0 TB", size.ToSizeSuffix());
        }

        /// <summary>Ensure the size suffix converts to petabytes string label as expected.</summary>
        [Fact]
        public void Test_SizeSuffix_AsPetabytes()
        {
            // Arrange
            long size = 1048576 * 1024;
            size *= 1024;
            size *= 1024;
            // we calculate it in this way, using multiplies, because we cannot declare a variable inline with the size that's required to test this.

            // Act/Assert
            Assert.Equal("1.0 PB", size.ToSizeSuffix());
        }

        /// <summary>Ensure the size suffix converts to exabytes string label as expected.</summary>
        [Fact]
        public void Test_SizeSuffix_AsExabytes()
        {
            // Arrange
            long size = 1048576 * 1024;
            size *= 1024;
            size *= 1024;
            size *= 1000;
            // we calculate it in this way, using multiplies, because we cannot declare a variable inline with the size that's required to test this.

            // Act/Assert
            Assert.Equal("1.0 EB", size.ToSizeSuffix());
        }

        /// <summary>Ensure the size suffix converts to kilobytes with decimal string label as expected.</summary>
        [Fact]
        public void Test_EpochTime()
        {
            // Arrange
            var dateToTest = DateTime.UtcNow;

            // Act
            var epoch = dateToTest.ToEpochTime();
            var result = epoch.ToDateTime();

            // Assert
            Assert.Equal(dateToTest, result);
        }
    }
}
