using System.ComponentModel;
using Cloud.Core.Testing;
using Xunit;
using FluentAssertions;
using Cloud.Core.Extensions;

namespace Cloud.Core.Tests
{
    [IsUnit]
    public class EnumExtensionsTest
    {
        /// <summary>Checks the enum is correctly converted to a list of strings.</summary>
        [Fact]
        public void Test_ToList_Enum()
        {
            // Arrange/Act
            var converted = EnumExtensions.ListFromEnum<MyTestEnum>();

            // Assert
            converted.Should().HaveCount(4);
            converted.Should().Contain("Zero");
            converted.Should().Contain("One");
            converted.Should().Contain("Two");
            converted.Should().Contain("Three");
        }

        /// <summary>Checks getting the enum description.</summary>
        [Fact]
        public void Test_ToDescription_WithDescription()
        {
            // Arrange/Act
            var first = MyTestEnum.One.ToDescription();
            var second = MyTestEnum.Two.ToDescription();

            // Assert
            Assert.Equal("oneDescription", first);
            Assert.Equal("twoDescription", second);
        }

        /// <summary>Checks the enum description defaults to a string version of the enum, when description has not been specified.</summary>
        [Fact]
        public void Test_ToDescription_WithoutDescription()
        {
            // Arrange/Act
            var third = MyTestEnum.Three.ToDescription();

            // Assert
            Assert.Equal("Three", third);
        }

        /// <summary>Checks conversion of an int to the equivilent enum.</summary>
        [Fact]
        public void Test_ConvertIntToEnum()
        {
            // Arrange/Act
            var third = 3.ConvertIntToEnum<MyTestEnum>();

            // Assert
            Assert.Equal(MyTestEnum.Three, third);
        }

        /// <summary>Checks conversion of a string to the equivilent enum.</summary>
        [Fact]
        public void Test_ConvertStringToEnum()
        {
            // Arrange/Act
            var third = "Three".ConvertStringToEnum<MyTestEnum>();

            // Assert
            Assert.Equal(MyTestEnum.Three, third);
        }

        /// <summary>Checks conversion of a string to the equivilent enum successfully.</summary>
        [Fact]
        public void Test_ConvertToEnum_Positive()
        {
            // Arrange/Act
            var third = EnumExtensions.ConvertToEnum<MyTestEnum>("Three");

            // Assert
            Assert.Equal(MyTestEnum.Three, third);
        }

        /// <summary>Checks conversion of a string to the equivilent enum usuccessfully and defaults to the first enum value.</summary>
        [Fact]
        public void Test_ConvertToEnum_Negative()
        {
            // Arrange/Act
            var third = EnumExtensions.ConvertToEnum<MyTestEnum>("");

            // Assert
            Assert.Equal(MyTestEnum.Zero, third);
        }

        private enum MyTestEnum
        {
            [Description("zeroDescription")]
            Zero = 0,
            [Description("oneDescription")]
            One = 1,
            [Description("twoDescription")]
            Two = 2,
            Three = 3
        }

    }
}
