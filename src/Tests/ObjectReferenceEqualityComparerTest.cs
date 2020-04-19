using Cloud.Core.Testing;
using Cloud.Core.Comparer;
using FluentAssertions;
using Xunit;

namespace Cloud.Core.Tests
{
    [IsUnit]
    public class ObjectReferenceEqualityComparerTest
    {
        [Fact]
        public void Test_EqualsOnInts()
        {
            var reference1 = (object)1;
            var reference2 = (object)1;

            ObjectReferenceEqualityComparer<object>.Default.Equals(reference1, reference2).Should().BeFalse();
        }

        [Fact]
        public void Test_EqualsOnStrings()
        {
            var reference1 = 123.ToString();
            var reference2 = 123.ToString();

            ObjectReferenceEqualityComparer<object>.Default.Equals(reference1, reference2).Should().BeFalse();
        }

        [Fact]
        public void Test_OnInts()
        {
            var reference1 = (object)1;
            var reference2 = (object)1;

            ObjectReferenceEqualityComparer<object>.Default.GetHashCode(reference1).Should().NotBe(ObjectReferenceEqualityComparer<object>.Default.GetHashCode(reference2));
        }

        [Fact]
        public void Test_OnStrings()
        {
            var reference1 = 123.ToString();
            var reference2 = 123.ToString();

            ObjectReferenceEqualityComparer<object>.Default.GetHashCode(reference1).Should().NotBe(ObjectReferenceEqualityComparer<object>.Default.GetHashCode(reference2));
        }
    }
}
