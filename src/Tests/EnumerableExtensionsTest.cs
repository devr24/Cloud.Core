using System.Collections.Generic;
using System.Threading.Tasks;
using Cloud.Core.Testing;
using FluentAssertions;
using Xunit;

namespace Cloud.Core.Tests
{
    [IsUnit]
    public class EnumerableExtensionsTest
    {
        /// <summary>Check contains equivalent will correct match an existing item.</summary>
        [Fact]
        public void Test_ContainsEquivalent_Positive()
        {
            // Arrange.
            IEnumerable<string> listTest = new List<string> { "one", "two", "three" };

            // Act
            var containsItem = listTest.ContainsEquivalent("one");

            // Assert
            Assert.True(containsItem);
        }

        /// <summary>Check contains equivalent will correctly identifies an existing item does not exist.</summary>
        [Fact]
        public void Test_ContainsEquivalent_Negative()
        {
            // Arrange
            IEnumerable<string> listTest = new List<string> { "one", "two", "three" };

            // Act
            var doesntContainEmtpy = listTest.ContainsEquivalent("");
            var doesntContainString = listTest.ContainsEquivalent("four");

            // Assert
            Assert.False(doesntContainEmtpy);
            Assert.False(doesntContainString);
        }

        /// <summary>Check that reading a list in batches returns the correct.</summary>
        [Fact]
        public void Test_Batch_GroupsItems()
        {   
            // Arrange
            IEnumerable<string> stringList = new List<string> { "one", "two", "three", "four", "five" };
            var countList= new List<int>();
            
            // Act
            // Recieve batches of 2 strings at a time. The final batch should return whatever is left
            foreach(var batch in stringList.Batch(2)) {
                countList.Add(batch.Length);
            }

            // Assert
            Assert.Equal(countList, new List<int>{ 2, 2, 1 });
        }

        /// <summary>Verify object type is or is not a system type as expected.</summary>
        [Fact]
        public async Task Test_Type_IsSystemType()
        {
            // Arrange
            var count = 0;

            // Act
            await GetEnumerableItems().ParallelForEachAsync((item) =>
            {
                count++;
                return Task.FromResult(true);
            });

            // Assert
            count.Should().Be(10);
        }

        private IEnumerable<AsyncSample> GetEnumerableItems()
        {
            for (int i = 0; i < 10; i++)
            {
                Task.Delay(i * 50).GetAwaiter().GetResult();
                yield return new AsyncSample { PropA = i.ToString() };
            }
        }

        private class AsyncSample
        {
            public string PropA { get; set; }
        }
    }
}
