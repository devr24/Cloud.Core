using System;
using System.Threading.Tasks;
using Cloud.Core.Services;
using Cloud.Core.Testing;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Cloud.Core.Tests
{
    [IsUnit]
    public class MonitorTest
    {
        /// <summary>Ensure monitor is created with int constuctor.</summary>
        [Fact]
        public async Task Test_MonitorTimer_IntConsturctor()
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

        /// <summary>Ensure monitor is created with config and logger constuctor.</summary>
        [Fact]
        public async Task Test_MonitorTimer_ConfigAndLoggerConstructor()
        {
            // Arrange.
            var mockLogger = new Mock<ILogger<MonitorService>>();
            var monitor = new MonitorService(new MonitorConfig(1), mockLogger.Object);

            // Act.
            monitor.BackgroundTimerTick += (elapsed) => {
                elapsed.Should().BeGreaterOrEqualTo(TimeSpan.FromSeconds(3));
            };

            await Task.Delay(3000);

            // Assert.
            monitor.AppName.Should().Be(AppDomain.CurrentDomain.FriendlyName);
        }

        /// <summary>Ensure monitor is created with config constuctor.</summary>
        [Fact]
        public async Task Test_MonitorTimer_ConfigConstructor()
        {
            // Arrange.
            var mockLogger = new Mock<ILogger<MonitorService>>();
            var monitor = new MonitorService(new MonitorConfig(1), null);

            // Act.
            monitor.BackgroundTimerTick += (elapsed) => {
                elapsed.Should().BeGreaterOrEqualTo(TimeSpan.FromSeconds(3));
            };

            await Task.Delay(3000);

            // Assert.
            monitor.AppName.Should().Be(AppDomain.CurrentDomain.FriendlyName);
        }

        /// <summary>Ensure monitor is created with both null constuctor.</summary>
        [Fact]
        public async Task Test_MonitorTimer_NullsConstructor()
        {
            // Arrange.
            var monitor = new MonitorService(null, null);

            // Act.
            monitor.BackgroundTimerTick += (elapsed) => {
                elapsed.Should().BeGreaterOrEqualTo(TimeSpan.FromSeconds(3));
            };

            await Task.Delay(3000);

            // Assert.
            monitor.AppName.Should().Be(AppDomain.CurrentDomain.FriendlyName);
        }

        /// <summary>Ensure monitor is created with no constuctor.</summary>
        [Fact]
        public void Test_MonitorTimer_NoConstructor()
        {
            // Arrange.
            var monitor = new MonitorService();

            // Assert.
            monitor.AppName.Should().Be(AppDomain.CurrentDomain.FriendlyName);
        }
    }
}
