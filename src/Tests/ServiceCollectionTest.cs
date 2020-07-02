using System;
using System.IO;
using System.Text;
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
    }
}
