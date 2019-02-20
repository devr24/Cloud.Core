using System.Collections.Generic;
using Cloud.Core.Testing;
using Xunit;

namespace Cloud.Core.Tests
{
    public class EnumerableExtensionsTest
    {
        [Fact, IsUnit]
        public void Test_ContainsEquivalent_Positive()
        {
            IEnumerable<string> listTest = new List<string> { "one", "two", "three" };
            Assert.True(listTest.ContainsEquivalent("one"));
        }

        [Fact, IsUnit]
        public void Test_ContainsEquivalent_Negative()
        {
            IEnumerable<string> listTest = new List<string> { "one", "two", "three" };
            Assert.False(listTest.ContainsEquivalent(""));
            Assert.False(listTest.ContainsEquivalent("four"));
        }
    }
}