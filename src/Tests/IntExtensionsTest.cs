using System;
using Cloud.Core.Testing;
using Xunit;

namespace Cloud.Core.Tests
{
    public class IntExtensionsTest
    {
        [Fact, IsUnit]
        public void Test_SizeSuffix_Throws()
        {
            long size = 0;
            Assert.Throws<ArgumentOutOfRangeException>(() => size.ToSizeSuffix(-1));
        }

        [Fact, IsUnit]
        public void Test_SizeSuffix_AsMinusBytes()
        {
            long size = -10;
            Assert.Equal("-10.0 bytes", size.ToSizeSuffix());
        }

        [Fact, IsUnit]
        public void Test_SizeSuffix_AsBytes()
        {
            long size = 0;
            Assert.Equal("0.0 bytes", size.ToSizeSuffix());
        }

        [Fact, IsUnit]
        public void Test_SizeSuffix_AsBytesWithDecimal()
        {
            long size = 0;
            Assert.Equal("0.00000 bytes", size.ToSizeSuffix(5));
        }

        [Fact, IsUnit]
        public void Test_SizeSuffix_AsKilobytes()
        {
            long size = 1024;
            Assert.Equal("1.0 KB", size.ToSizeSuffix());
        }

        [Fact, IsUnit]
        public void Test_SizeSuffix_AsKilobytesWithDecimal()
        {
            long size = 1024;
            Assert.Equal("1.00000 KB", size.ToSizeSuffix(5));
        }

        [Fact, IsUnit]
        public void Test_SizeSuffix_AsMegabytes()
        {
            long size = 1024 * 1024;
            Assert.Equal("1.0 MB", size.ToSizeSuffix());
        }

        [Fact, IsUnit]
        public void Test_SizeSuffix_AsGigabytes()
        {
            long size = 1048576 * 1024;
            Assert.Equal("1.0 GB", size.ToSizeSuffix());
        }

        [Fact, IsUnit]
        public void Test_SizeSuffix_AsTerabytes()
        {
            long size = 1048576 * 1024;
            size = size * 1024;
            Assert.Equal("1.0 TB", size.ToSizeSuffix());
        }

        [Fact, IsUnit]
        public void Test_SizeSuffix_AsPetabytes()
        {
            long size = 1048576 * 1024;
            size = size * 1024;
            size = size * 1024;
            Assert.Equal("1.0 PB", size.ToSizeSuffix());
        }

        [Fact, IsUnit]
        public void Test_SizeSuffix_AsExabytes()
        {
            long size = 1048576 * 1024;
            size = size * 1024;
            size = size * 1024;
            size = size * 1000;
            Assert.Equal("1.0 EB", size.ToSizeSuffix());
        }
    }
}
