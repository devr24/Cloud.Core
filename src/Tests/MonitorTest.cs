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
            var hasTicked = false;
            TimeSpan? timeTicked = null;
            var monitor = new MonitorService(3);
            
            // Act.
            monitor.BackgroundTimerTick += (elapsed) => {
                hasTicked = true;
                if (timeTicked == null)
                    timeTicked = elapsed;
            };

            await Task.Delay(7000);

            // Assert.
            monitor.AppName.Should().Be(AppDomain.CurrentDomain.FriendlyName);
            hasTicked.Should().BeTrue();
            timeTicked.Value.Should().BeGreaterOrEqualTo(TimeSpan.FromSeconds(3));
        }
    }
}
