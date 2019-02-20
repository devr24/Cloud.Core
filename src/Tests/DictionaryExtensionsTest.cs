using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cloud.Core.Testing;
using Xunit;

namespace Cloud.Core.Tests
{
    public class DictionaryExtensionsTest
    {
        [Fact, IsUnit]
        public void Test_Release()
        {
            var testDict = new Dictionary<string, MemoryStream>
            {
                { "1", new MemoryStream() },
                { "2", new MemoryStream() }
            };

            Assert.Equal(2, testDict.Count);

            testDict.Release();

            Assert.Empty(testDict);
        }

        [Fact, IsUnit]
        public void Test_ToString()
        {
            var testDict = new Dictionary<string, string>
            {
                { "1", "1" },
                { "2", "2" }
            };

            Assert.Equal("1=1;2=2", testDict.ToDelimitedString());
        }
    }
}
