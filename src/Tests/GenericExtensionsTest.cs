using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Cloud.Core.Extensions;
using Cloud.Core.Testing;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Cloud.Core.Tests
{
    [IsUnit]
    public class GenericExtensionsTest
    {
        /// <summary>Verify object type is changed as expected.</summary>
        [Fact]
        public void Test_Object_ChangeType()
        {
            // Arrange
            var mytest = "0";

            // Act
            var changed = mytest.ChangeType<int>();

            // Assert
            changed.GetType().Should().Be(typeof(int));
        }

        /// <summary>Verify list object is null or default check works as expected.</summary>
        [Fact]
        public void Test_ListObject_IsNullOrDefault()
        {
            // Arrange
            List<string> obj = null;

            // Act
            var result = obj.IsNullOrDefault();

            // Assert
            Assert.True(result);
        }

        /// <summary>Verify object is null or default check works as expected.</summary>
        [Fact]
        public void Test_NullObject_IsNullOrDefault()
        {
            // Arrange
            object obj = null;

            // Act
            var result = obj.IsNullOrDefault();

            // Assert
            Assert.True(result);
        }

        /// <summary>Verify non-null list is null or default check works as expected.</summary>
        [Fact]
        public void Test_NonNullList_IsNullOrDefault()
        {
            // Arrange
            var obj = new List<string> { "test" };

            // Act
            var result = obj.IsNullOrDefault();

            // Assert
            Assert.False(result);
        }

        /// <summary>Verify non-null string is null or default check works as expected.</summary>
        [Fact]
        public void Test_StringObject_IsNullOrDefault()
        {
            // Arrange
            object obj = "string";

            // Act
            var result = obj.IsNullOrDefault();

            // Assert
            Assert.False(result);
        }

        /// <summary>Verify null list throws if null or default works as expected.</summary>
        [Fact]
        public void Test_NullList_ThrowIfNullOrDefault()
        {
            // Act
            List<string> obj = null;

            // Act/Assert
            Assert.Throws<ArgumentNullException>(() => obj.ThrowIfNullOrDefault());
        }

        /// <summary>Verify null object throws if null or default works as expected.</summary>
        [Fact]
        public void Test_NullObject_ThrowIfNullOrDefault()
        {
            // Act
            object obj = null;

            // Act/Assert
            Assert.Throws<ArgumentNullException>(() => obj.ThrowIfNullOrDefault());
        }

        /// <summary>Verify a sub array can be taken from a source array.</summary>
        [Fact]
        public void Test_Array_SubArray()
        {
            // Arrange
            var testData = new string[] { "1", "2", "3", "4", "5" };

            // Act
            var subArray = testData.SubArray(0, 3);

            // Assert
            subArray.Length.Should().Be(3);
            subArray[0].Should().Be("1");
            subArray[1].Should().Be("2");
            subArray[2].Should().Be("3");
        }
    }
}
