using System.Collections.Generic;
using Cloud.Core.Testing;
using Xunit;

namespace Cloud.Core.Tests
{
    [IsUnit]
    public class EnumerableExtensionsTest
    {
        [Fact]
        public void Test_ContainsEquivalent_Positive()
        {
            IEnumerable<string> listTest = new List<string> { "one", "two", "three" };
            Assert.True(listTest.ContainsEquivalent("one"));
        }

        [Fact]
        public void Test_ContainsEquivalent_Negative()
        {
            IEnumerable<string> listTest = new List<string> { "one", "two", "three" };
            Assert.False(listTest.ContainsEquivalent(""));
            Assert.False(listTest.ContainsEquivalent("four"));
        }

        [Fact]
        public void Test_Batch_ReturnsMoreThanOneBatch()
        {            
            IEnumerable<string> stringList = new List<string> { "one", "two", "three", "four", "five" };

            var countList= new List<int>();

            //Recieve batches of 2 strings at a time. The final batch should return whatever is left
            foreach(var batch in stringList.Batch(2)){
                countList.Add(batch.Length);
            }

            Assert.Equal(countList, new List<int>{ 2, 2, 1 });
        }
    }
}
