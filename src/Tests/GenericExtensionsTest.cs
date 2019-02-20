using System;
using System.Collections.Generic;
using Cloud.Core.Testing;
using Xunit;

namespace Cloud.Core.Tests
{
    public class GenericExtensionsTest
    {
        [Fact, IsUnit]
        public void Test_IsNullOrDefault_NullList()
        {
            List<string> obj = null;
            Assert.True(obj.IsNullOrDefault());
        }

        [Fact, IsUnit]
        public void Test_IsNullOrDefault_NullObj()
        {
            object obj = null;
            Assert.True(obj.IsNullOrDefault());
        }

        [Fact, IsUnit]
        public void Test_IsNullOrDefault_NullList_Negative()
        {
            List<string> obj = new List<string> { "test" };
            Assert.False(obj.IsNullOrDefault());
        }

        [Fact, IsUnit]
        public void Test_IsNullOrDefault_NullObj_Negative()
        {
            object obj = "string";
            Assert.False(obj.IsNullOrDefault());
        }

        [Fact, IsUnit]
        public void Test_ThrowIfNullOrDefault_ThrowNullList()
        {
            List<string> obj = null;
            Assert.Throws<ArgumentNullException>(() => obj.ThrowIfNullOrDefault());
        }

        [Fact, IsUnit]
        public void Test_ThrowIfNullOrDefault_ThrowNullObj()
        {
            object obj = null;
            Assert.Throws<ArgumentNullException>(() => obj.ThrowIfNullOrDefault());
        }
    }
}
