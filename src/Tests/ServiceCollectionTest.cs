using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cloud.Core.Testing;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cloud.Core.Tests
{
    [IsUnit]
    public class ServiceCollectionExtensionsTest
    {
        /// <summary>Check the string is converted into stream and back into string as expected.</summary>
        [Fact]
        public void Test_ServiceCollectionExtension_AddNamedInstance()
        {
            // Arrange/Act.
            var services = new ServiceCollection();
            services.AddFactoryIfNotAdded<IBlobStorage>();
            var exists = services.ContainsService(typeof(NamedInstanceFactory<IBlobStorage>));
            var doestNotExist = services.ContainsService(typeof(NamedInstanceFactory<IReactiveMessenger>));

            // Assert.
            exists.Should().BeTrue();
            doestNotExist.Should().BeFalse();
        }

        /// <summary>Check the string is converted into stream and back into string as expected.</summary>
        [Fact]
        public void Test_ServiceCollectionExtension_AddEmailProviderInstance()
        {
            // Arrange/Act.
            var services = new ServiceCollection();

            var doestNotExist = services.ContainsService(typeof(NamedInstanceFactory<IEmailProvider>));
            services.AddEmailProvider<FakeEmailProvider>();
            var existsFactory = services.ContainsService(typeof(NamedInstanceFactory<IEmailProvider>));
            var exists = services.ContainsService(typeof(IEmailProvider));

            // Assert.
            exists.Should().BeTrue();
            existsFactory.Should().BeTrue();
            doestNotExist.Should().BeFalse();
        }


        /// <summary>Check the string is converted into stream and back into string as expected.</summary>
        [Fact]
        public void Test_ServiceCollectionExtension_AddSmsProviderInstance()
        {
            // Arrange/Act.
            var services = new ServiceCollection();

            var doestNotExist = services.ContainsService(typeof(NamedInstanceFactory<ISmsProvider>));
            services.AddSmsProvider<FakeSmsProvider>();
            var existsFactory = services.ContainsService(typeof(NamedInstanceFactory<ISmsProvider>));
            var exists = services.ContainsService(typeof(ISmsProvider));

            // Assert.
            exists.Should().BeTrue();
            existsFactory.Should().BeTrue();
            doestNotExist.Should().BeFalse();
        }

        private class FakeEmailProvider : IEmailProvider
        {
            public string Name { get; set; } = "EmailExample";

            public bool Send(IEmailMessage email)
            {
                return true;
            }

            public Task<bool> SendAsync(IEmailMessage email)
            {
                return Task.FromResult(true);
            }
        }

        private class FakeSmsProvider : ISmsProvider
        {
            public string Name { get; set; } = "SmsExample";

            public bool Send(FakeSmsMessage sms)
            {
                return true;
            }

            public bool Send(ISmsMessage sms)
            {
                return true;
            }
            public Task<bool> SendAsync(ISmsMessage sms)
            {
                return Task.FromResult(true);
            }
        }

        private class FakeEmailMessage : IEmailMessage
        {
            public List<IEmailRecipient> To => throw new System.NotImplementedException();

            public string Subject { get; set; }
            public string TemplateName { get; set; }
            public string Content { get; set; }
            public bool IsPlainText { get; set; }

            public List<IEmailAttachment> Attachments => throw new System.NotImplementedException();
        }

        private class FakeSmsMessage : ISmsMessage
        {
            public FakeSmsMessage()
            {
                var a = new List<FakeSmsLink>();
                List<ISmsLink> links = a.Select(l => l as ISmsLink).ToList<ISmsLink>();
                Links = links;
            }

            public List<string> To { get; set; }
            public string Text { get; set; }

            public List<ISmsLink> Links { get; }

            public string FullContent => Text;
        }

        public class FakeSmsLink : ISmsLink
        {
            public string Title { get;set; }
            public Uri Link { get; set; }
        }
    }
}
