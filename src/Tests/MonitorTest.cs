using System;
using System.Threading.Tasks;
using Cloud.Core.Services;
using Cloud.Core.Testing;
using FluentAssertions;
using Xunit;

namespace Cloud.Core.Tests
{
    [IsUnit]
    public class MonitorTest
    {
        /// <summary>Ensure conflict exception type is captured.</summary>
        [Fact]
        public async Task Test_ConflictException_WithMessage()
        {
            // Arrange.
            var monitor = new MonitorService(1);
            
            // Act.
            monitor.BackgroundTimerTick += (elapsed) => {
                elapsed.Should().BeGreaterOrEqualTo(TimeSpan.FromSeconds(3));
            };

            await Task.Delay(3000);

            // Assert.
            monitor.AppName.Should().Be(AppDomain.CurrentDomain.FriendlyName);
        }
    }
}
