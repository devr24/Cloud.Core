using System;
using System.Collections.Generic;
using Cloud.Core.Notification;
using Cloud.Core.Testing;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Cloud.Core.Tests
{
    [IsUnit]
    public class NotificationTest
    {
        /// <summary>Verify the GetTemplateObject Type returns the correct type.</summary>
        [Fact]
        public void Test_EmailTemplateMessage_GetSetObject()
        {
            // Arrange
            var templatedEmail = new EmailTemplateMessage { 
                Subject = "subject",
                TemplateId = "templateid",
                TemplateObject = new Test
                {
                    Prop1 = "A",
                    Prop2 = "B",
                    Prop3 = "C"
                }
            };
            templatedEmail.To.AddRange(new List<string> { 
                "ABC",
                "123"
            });

            // Act
            var serialized = JsonConvert.SerializeObject(templatedEmail.TemplateObject);

            // Assert
            templatedEmail.TemplateObjectAsJson().Should().Be(serialized);
            templatedEmail.GetTemplateObjectType().Should().Be(typeof(Test));
        }

        private class Test
        {
            public string Prop1 { get; set; }
            public string Prop2 { get; set; }
            public string Prop3 { get; set; }
        }
    }
}
