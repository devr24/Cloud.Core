using System;
using System.Collections.Generic;
using System.Text;
using Cloud.Core.Notification;
using Cloud.Core.Testing;
using FluentAssertions;
using Xunit;

namespace Cloud.Core.Tests
{
    [IsUnit]
    public class ProviderTests
    {
        [Fact]
        public void Test_EmailMessage_Creation()
        {
            // Arrange
            var emailMessage = new EmailMessage { 
                Content = "test",
                Subject = "test"
            };

            // Act
            emailMessage.To.AddRange(new List<string> { 
                "a@a.com",
                "b@b.com"
            });
            emailMessage.Attachments.AddRange(new List<EmailAttachment> { 
                new EmailAttachment { 
                    ContentType = "text/plain",
                    Name = "example.txt",
                    Content = "text".ConvertToStream(Encoding.UTF8)
                }
            });

            // Assert
            emailMessage.Content.Should().Be("test");
            emailMessage.Subject.Should().Be("test");
            emailMessage.To.Count.Should().Be(2);
            emailMessage.Attachments.Count.Should().Be(1);
        }

        [Fact]
        public void Test_EmailTemplatedMessage_Creation()
        {
            // Arrange
            var emailMessage = new EmailTemplateMessage
            {
                TemplateId = "test",
                Subject = "test",
                TemplateObject = new { 
                    PropAKey= "PropAVal",
                    PropBKey = 10,
                    PropCKey = new string[] { "a","b","c" }
                }
            };

            // Act
            emailMessage.To.AddRange(new List<string> {
                "a@a.com",
                "b@b.com"
            });
            emailMessage.Attachments.AddRange(new List<EmailAttachment> {
                new EmailAttachment {
                    ContentType = "text/plain",
                    Name = "example.txt",
                    Content = "text".ConvertToStream(Encoding.UTF8)
                }
            });
            var templateObjectJson = emailMessage.TemplateObjectAsJson();

            // Assert
            emailMessage.TemplateId.Should().Be("test");
            emailMessage.Subject.Should().Be("test");
            emailMessage.To.Count.Should().Be(2);
            emailMessage.Attachments.Count.Should().Be(1);
            templateObjectJson.Should().NotBeNullOrEmpty();
            templateObjectJson.Should().Be("{\"PropAKey\":\"PropAVal\",\"PropBKey\":10,\"PropCKey\":[\"a\",\"b\",\"c\"]}");
        }

        [Fact]
        public void Test_SmsMessage_Creation()
        {
            // Arrange
            var smsMessage = new SmsMessage
            {
                Text = "test"
            };
            
            // Act
            smsMessage.To.AddRange(new List<string> {
                "+447222222222",
                "+447333333333"
            });
            smsMessage.Links.AddRange(new List<SmsLink> { 
                new SmsLink { Link = new Uri("https://www.google.com"), Title = "Google" }
            });
            var expectedContent = $"test\nGoogle: https://www.google.com/";

            // Assert
            smsMessage.Text.Should().Be("test");
            smsMessage.To.Count.Should().Be(2);
            smsMessage.Links.Count.Should().Be(1);
            smsMessage.FullContent.Should().BeEquivalentTo(expectedContent);
        }
    }
}
