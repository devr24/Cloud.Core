using Cloud.Core.Exceptions;
using Cloud.Core.Testing;
using FluentAssertions;
using Xunit;

namespace Cloud.Core.Tests
{
    [IsUnit]
    public class ExceptionTest
    {
        [Fact]
        public void Test_ConflictException_WithMessage()
        {
            try
            {
                throw new ConflictException("Test exception");
            }
            catch (ConflictException cex)
            {
                cex.GetType().Should().Be(typeof(ConflictException));
            }
        }

        [Fact]
        public void Test_ConflictException_WithMessageAndInnerEx()
        {
            try
            {
                throw new ConflictException("Test exception", new System.Exception());
            }
            catch (ConflictException cex)
            {
                cex.GetType().Should().Be(typeof(ConflictException));
            }
        }

        [Fact]
        public void Test_ConflictException()
        {
            try
            {
                throw new ConflictException();
            }
            catch (ConflictException cex)
            {
                cex.GetType().Should().Be(typeof(ConflictException));
            }
        }

        [Fact]
        public void Test_EntityDisabledException_WithEntityName()
        {
            try
            {
                throw new EntityDisabledException("Test exception");
            }
            catch (EntityDisabledException cex)
            {
                cex.GetType().Should().Be(typeof(EntityDisabledException));
            }
        }

        [Fact]
        public void Test_EntityDisabledException_WithMessage()
        {
            try
            {
                throw new EntityDisabledException("entity name", "some message");
            }
            catch (EntityDisabledException cex)
            {
                cex.GetType().Should().Be(typeof(EntityDisabledException));
            }
        }

        [Fact]
        public void Test_EntityDisabledException_WithMessageAndEx()
        {
            try
            {
                var entity = new EntityMessageCount()
                {
                    ActiveEntityCount = 1,
                    ErroredEntityCount = 1
                };
                entity.ActiveEntityCount.Should().Be(1);
                entity.ErroredEntityCount.Should().Be(1);
                throw new EntityDisabledException("EntityName", "some message", new System.Exception());
            }
            catch (EntityDisabledException cex)
            {
                cex.GetType().Should().Be(typeof(EntityDisabledException));
                cex.EntityName.Should().Be("EntityName");
            }
        }

        [Fact]
        public void Test_EntityFullException_WithEntityName()
        {
            try
            {
                throw new EntityFullException("Test exception");
            }
            catch (EntityFullException cex)
            {
                cex.GetType().Should().Be(typeof(EntityFullException));
            }
        }

        [Fact]
        public void Test_EntityFullException_WithMessage()
        {
            try
            {
                throw new EntityFullException("EntityName", "Test exception");
            }
            catch (EntityFullException cex)
            {
                cex.GetType().Should().Be(typeof(EntityFullException));
            }
        }

        [Fact]
        public void Test_EntityFullException_WithMessageAndEx()
        {
            try
            {
                throw new EntityFullException("EntityName", "Test exception", new System.Exception());
            }
            catch (EntityFullException cex)
            {
                cex.GetType().Should().Be(typeof(EntityFullException));
                cex.EntityName.Should().Be("EntityName");
            }
        }

        [Fact]
        public void Test_EntityFullException_WithProperties()
        {
            try
            {
                throw new EntityFullException("EntityName", "Test exception", 10000, 100000, new System.Exception());
            }
            catch (EntityFullException cex)
            {
                cex.GetType().Should().Be(typeof(EntityFullException));
                cex.EntityName.Should().Be("EntityName");
                cex.MaxSizeBytes.Should().Be(100000);
                cex.CurrentSizeBytes.Should().Be(10000);
                cex.PercentUsed.Should().Be(10);
            }
        }
    }
}
