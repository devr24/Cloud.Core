using Cloud.Core.Exceptions;
using Cloud.Core.Testing;
using FluentAssertions;
using Xunit;

namespace Cloud.Core.Tests
{
    [IsUnit]
    public class ExceptionTest
    {
        /// <summary>Ensure conflict exception type is captured.</summary>
        [Fact]
        public void Test_ConflictException_WithMessage()
        {
            try
            {
                // Arrange/Act
                throw new ConflictException("Test exception");
            }
            catch (ConflictException cex)
            {
                // Assert
                cex.GetType().Should().Be(typeof(ConflictException));
                cex.Message.Should().Be("Test exception");
            }
        }

        /// <summary>Ensure conflict exception type is captured with inner exception.</summary>
        [Fact]
        public void Test_ConflictException_WithMessageAndInnerEx()
        {
            try
            {
                // Arrange/Act
                throw new ConflictException("Test exception", new System.Exception());
            }
            catch (ConflictException cex)
            {
                // Assert
                cex.GetType().Should().Be(typeof(ConflictException));
            }
        }

        /// <summary>Ensure conflict exception type is captured.</summary>
        [Fact]
        public void Test_ConflictException()
        {
            try
            {
                // Arrange/Act
                throw new ConflictException();
            }
            catch (ConflictException cex)
            {
                // Assert
                cex.GetType().Should().Be(typeof(ConflictException));
            }
        }

        /// <summary>Ensure entity disabled exception type with entity name is captured.</summary>
        [Fact]
        public void Test_EntityDisabledException_WithEntityName()
        {
            try
            {
                // Arrange/Act
                throw new EntityDisabledException("Test exception");
            }
            catch (EntityDisabledException edx)
            {
                // Assert
                edx.GetType().Should().Be(typeof(EntityDisabledException));
            }
        }

        /// <summary>Ensure entity disabled exception type is captured with entity name and message.</summary>
        [Fact]
        public void Test_EntityDisabledException_WithMessage()
        {
            try
            {
                // Arrange/Act
                throw new EntityDisabledException("entity name", "some message");
            }
            catch (EntityDisabledException edx)
            {
                // Assert
                edx.GetType().Should().Be(typeof(EntityDisabledException));
                edx.EntityName.Should().Be("entity name");
                edx.Message.Should().Be("some message");
            }
        }

        /// <summary>Ensure entity disabled exception type is captured with message and entity name.</summary>
        [Fact]
        public void Test_EntityDisabledException_WithMessageAndEx()
        {
            try
            {
                // Arrange
                var entity = new EntityMessageCount()
                {
                    ActiveEntityCount = 1,
                    ErroredEntityCount = 1
                };
                entity.ActiveEntityCount.Should().Be(1);
                entity.ErroredEntityCount.Should().Be(1);

                // Act
                throw new EntityDisabledException("EntityName", "some message", new System.Exception());
            }
            catch (EntityDisabledException edx)
            {
                // Assert
                edx.GetType().Should().Be(typeof(EntityDisabledException));
                edx.EntityName.Should().Be("EntityName");
                edx.Message.Should().Be("some message");
            }
        }

        /// <summary>Ensure entity full exception type with entity name is captured.</summary>
        [Fact]
        public void Test_EntityFullException_WithEntityName()
        {
            try
            {
                // Arrange/Act
                throw new EntityFullException("Test exception");
            }
            catch (EntityFullException efx)
            {
                // Assert
                efx.GetType().Should().Be(typeof(EntityFullException));
                efx.EntityName.Should().Be("Test exception");
            }
        }

        /// <summary>Ensure entity full exception type is captured.</summary>
        [Fact]
        public void Test_EntityFullException_WithMessage()
        {
            try
            {
                // Arrange/Act
                throw new EntityFullException("EntityName", "Test exception");
            }
            catch (EntityFullException efx)
            {
                // Assert
                efx.GetType().Should().Be(typeof(EntityFullException));
                efx.Message.Should().Be("Test exception");
            }
        }

        /// <summary>Ensure entity full exception type is captured with message.</summary>
        [Fact]
        public void Test_EntityFullException_WithMessageAndEx()
        {
            try
            {
                // Arrange/Act
                throw new EntityFullException("EntityName", "Test exception", new System.Exception());
            }
            catch (EntityFullException efx)
            {
                // Assert
                efx.GetType().Should().Be(typeof(EntityFullException));
                efx.EntityName.Should().Be("EntityName");
                efx.Message.Should().Be("Test exception");
            }
        }

        /// <summary>Ensure entity full exception type is captured with associated properties.</summary>
        [Fact]
        public void Test_EntityFullException_WithProperties()
        {
            try
            {
                // Arrange/Act
                throw new EntityFullException("EntityName", "Test exception", 10000, 100000, new System.Exception());
            }
            catch (EntityFullException efx)
            {
                // Assert
                efx.GetType().Should().Be(typeof(EntityFullException));
                efx.EntityName.Should().Be("EntityName");
                efx.MaxSizeBytes.Should().Be(100000);
                efx.CurrentSizeBytes.Should().Be(10000);
                efx.PercentUsed.Should().Be(10);
            }
        }
    }
}
