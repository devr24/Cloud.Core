using System.ComponentModel;
using Cloud.Core.Testing;
using Xunit;
using FluentAssertions;
using System;

namespace Cloud.Core.Tests
{
    [IsUnit]
    public class EnumExtensionsTest
    {
        private enum MyTestEnum
        {
            [Description("zero")]
            Zero = 0,
            [Description("one")]
            One = 1,
            [Description("two")]
            Two = 2,
            Three = 3
        }

        [Fact]
        public void Test_ToList_Enum()
        {
            var converted = EnumExtensions.ListFromEnum<MyTestEnum>();

            converted.Should().HaveCount(4);
            converted.Should().Contain("Zero");
            converted.Should().Contain("One");
            converted.Should().Contain("Two");
            converted.Should().Contain("Three");
        }

        [Fact]
        public void Test_ToDescription_WithDescription()
        {
            Assert.Equal("one", MyTestEnum.One.ToDescription());
            Assert.Equal("two", MyTestEnum.Two.ToDescription());
        }

        [Fact]
        public void Test_ToDescription_WithoutDescription()
        {
            Assert.Equal("Three", MyTestEnum.Three.ToDescription());
        }

        [Fact]
        public void Test_ConvertIntToEnum()
        {
            Assert.Equal(MyTestEnum.Three, 3.ConvertIntToEnum<MyTestEnum>());
        }

        [Fact]
        public void Test_ConvertStringToEnum()
        {
            Assert.Equal(MyTestEnum.Three, "Three".ConvertStringToEnum<MyTestEnum>());
        }

        [Fact]
        public void Test_ConvertToEnum_Positive()
        {
            Assert.Equal(MyTestEnum.Three, EnumExtensions.ConvertToEnum<MyTestEnum>("Three"));
        }

        [Fact]
        public void Test_ConvertToEnum_Negative()
        {
            Assert.Equal(MyTestEnum.Zero, EnumExtensions.ConvertToEnum<MyTestEnum>(""));
        }
    }
}
