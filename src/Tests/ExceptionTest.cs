using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using Cloud.Core.Exceptions;
using Cloud.Core.Testing;
using Cloud.Core.Validation;
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

        /// <summary>Tests throwing of a validation exception with the default constructor.</summary>
        [Fact]
        public void Test_ValidationException_DefaultConstructor()
        {
            try
            {
                throw new ValidateException();
            }
            catch (ValidateException vx)
            {
                // Assert
                vx.GetType().Should().Be(typeof(ValidateException));
            }
        }

        /// <summary>Tests throwing of a validation exception with message.</summary>
        [Fact]
        public void Test_ValidationException_WithValidationMessage()
        {
            try
            {
                throw new ValidateException("example");
            }
            catch (ValidateException vx)
            {
                // Assert
                vx.GetType().Should().Be(typeof(ValidateException));
                vx.Message.Should().Be("example");
            }
        }

        /// <summary>Tests throwing of a validation exception with inner exception.</summary>
        [Fact]
        public void Test_ValidationException_WithValidationInnerException()
        {
            try
            {
                var errors = new List<ValidationResult>();
                throw new ValidateException("example", new System.Exception("inner example"));
            }
            catch (ValidateException vx)
            {
                // Assert
                vx.GetType().Should().Be(typeof(ValidateException));
                vx.Message.Should().Be("example");
                vx.InnerException.Message.Should().Be("inner example");
            }
        }

        /// <summary>Tests throwing of a validation exception with validation result.</summary>
        [Fact]
        public void Test_ValidationException_WithValidationResult()
        {
            try
            {
                var errors = new List<ValidationResult> { 
                    new ValidationResult("example")
                };
                var errorResult = new ValidateResult(errors);
                throw new ValidateException(errorResult);
            }
            catch (ValidateException vx)
            {
                // Assert
                vx.GetType().Should().Be(typeof(ValidateException));
                vx.Errors.First().ErrorMessage.Should().Be("example");
            }
        }

        /// <summary>Tests throwing of a template exception with the default constructor.</summary>
        [Fact]
        public void Test_TemplateMappingException_DefaultConstructor()
        {
            try
            {
                throw new TemplateMappingException("example", "template1", true, null, null);
            }
            catch (TemplateMappingException tmx)
            {
                // Assert
                tmx.GetType().Should().Be(typeof(TemplateMappingException));
                tmx.Message.Should().Be("example");
                tmx.TemplateName.Should().Be("template1");
                tmx.TemplateFound.Should().BeTrue();
                tmx.TemplateKeys.Should().BeNull();
                tmx.ModelKeyValues.Should().BeNull();
            }
        }

        /// <summary>Tests throwing of a template exception with an inner exception.</summary>
        [Fact]
        public void Test_TemplateMappingException_WithException()
        {
            try
            {
                throw new TemplateMappingException("example", new System.Exception("inner example"), "template1", true, null, null);
            }
            catch (TemplateMappingException tmx)
            {
                // Assert
                tmx.GetType().Should().Be(typeof(TemplateMappingException));
                tmx.InnerException.Message.Should().Be("inner example");
                tmx.Message.Should().Be("example");
                tmx.TemplateName.Should().Be("template1");
                tmx.TemplateFound.Should().BeTrue();
                tmx.TemplateKeys.Should().BeNull();
                tmx.ModelKeyValues.Should().BeNull();
            }
        }

        /// <summary>Tests throwing of a request failed exception with the default constructor.</summary>
        [Fact]
        public void Test_RequestFailedException_DefaultConstructor()
        {
            try
            {
                throw new RequestFailedException<string>(HttpStatusCode.BadRequest, "failed", "test");
            }
            catch (RequestFailedException<string> rx)
            {
                // Assert
                rx.GetType().Should().Be(typeof(RequestFailedException<string>));
                rx.Message.Should().Be("Request failed");
                rx.ResponseStatusCode.Should().Be(400);
                rx.ResponseBody.Should().Be("failed");
                rx.RequestObject.Should().Be("test");
            }
        }

        /// <summary>Tests throwing of a request failed exception with message.</summary>
        [Fact]
        public void Test_RequestFailedException_WithMessage()
        {
            try
            {
                throw new RequestFailedException<string>("example", HttpStatusCode.BadRequest, "failed", "test");
            }
            catch (RequestFailedException<string> rx)
            {
                // Assert
                rx.GetType().Should().Be(typeof(RequestFailedException<string>));
                rx.Message.Should().Be("example");
                rx.InnerException.Message.Should().Be("inner example");
                rx.ResponseStatusCode.Should().Be(400);
                rx.ResponseBody.Should().Be("failed");
                rx.RequestObject.Should().Be("test");
            }
        }

        /// <summary>Tests throwing of a request failed exception with exception.</summary>
        [Fact]
        public void Test_RequestFailedException_WithException()
        {
            try
            {
                throw new RequestFailedException<string>("example", new System.Exception("inner example"), HttpStatusCode.BadRequest, "failed", "test");
            }
            catch (RequestFailedException<string> rx)
            {
                // Assert
                rx.GetType().Should().Be(typeof(RequestFailedException<string>));
                rx.Message.Should().Be("example");
                rx.ResponseStatusCode.Should().Be(400);
                rx.ResponseBody.Should().Be("failed");
                rx.RequestObject.Should().Be("test");
            }
        }
    }
}
