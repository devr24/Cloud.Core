using System.Threading.Tasks;
using Cloud.Core.Extensions;
using Cloud.Core.Notification;
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

            public bool UseProviderTemplates => true;

            public EmailSendResult Send(EmailMessage email)
            {
                return new EmailSendResult { Success = true };
            }

            public EmailSendResult Send(EmailTemplateMessage templatedEmail)
            {
                return new EmailSendResult { Success = true };
            }

            public Task<EmailSendResult> SendAsync(EmailMessage email)
            {
                return Task.FromResult(new EmailSendResult { Success = true });
            }

            public Task<EmailSendResult> SendAsync(EmailTemplateMessage templatedEmail)
            {
                return Task.FromResult(new EmailSendResult { Success = true });
            }
        }

        private class FakeSmsProvider : ISmsProvider
        {
            public string Name { get; set; } = "SmsExample";

            public bool Send(SmsMessage sms)
            {
                return true;
            }

            public Task<bool> SendAsync(SmsMessage sms)
            {
                return Task.FromResult(true);
            }
        }
    }
}
