using System;
using System.Diagnostics;
using System.Timers;
using Microsoft.Extensions.Logging;

namespace Cloud.Core.Services
{
    /// <summary>Monitor Configuration</summary>
    public class MonitorConfig
    {
        /// <summary>Initializes a new instance of the <see cref="MonitorConfig"/> class.</summary>
        /// <param name="frequencySeconds">The frequency seconds.</param>
        public MonitorConfig(int frequencySeconds = 60)
        {
            MonitorFrequencySeconds = frequencySeconds;
        }

        /// <summary>Gets the monitor frequency seconds.</summary>
        /// <value>The monitor frequency seconds.</value>
        public int MonitorFrequencySeconds { get; }
    }

    /// <summary>Monitor Service class tracks a constant tick throughout the application.</summary>
    public class MonitorService
    {
        private readonly ILogger<MonitorService> _logger;
        private readonly Stopwatch _elapsedTime = new Stopwatch();
        private readonly MonitorConfig _config;

        /// <summary>Gets the name of the application.</summary>
        /// <value>The name of the application.</value>
        public string AppName => AppDomain.CurrentDomain.FriendlyName;

        /// <summary>
        /// Gets or sets the background timer tick action event - used to hook into the background timer tick to allow custom logs to be written.
        /// </summary>
        /// <value>The background timer tick.</value>
        public Action<TimeSpan> BackgroundTimerTick { get; set; } = null;

        /// <summary>Initializes a new instance of the <see cref="MonitorService"/> class.</summary>
        public MonitorService()
        {
            _config = new MonitorConfig();
            StartMonitor();
        }

        /// <summary>Initializes a new instance of the <see cref="MonitorService"/> class.</summary>
        /// <param name="frequencySeconds">The frequency seconds.</param>
        public MonitorService(int frequencySeconds = 60)
        {
            _config = new MonitorConfig(frequencySeconds);
            StartMonitor();
        }

        /// <summary>Initializes a new instance of the <see cref="MonitorService"/> class.</summary>
        /// <param name="logger">The logger.</param>
        public MonitorService(ILogger<MonitorService> logger)
        {
            _config = new MonitorConfig();
            _logger = logger;

            StartMonitor();
        }

        /// <summary>Initializes a new instance of the <see cref="MonitorService"/> class.</summary>
        /// <param name="config">Monitor configuration.</param>
        /// <param name="logger">The logger.</param>
        public MonitorService(MonitorConfig config, ILogger<MonitorService> logger)
        {
            if (config == null)
                config = new MonitorConfig();
            _config = config;
            _logger = logger;

            StartMonitor();
        }

        private void StartMonitor()
        {
            _elapsedTime.Start();
            var monitor = new Timer();
            monitor.Elapsed += (time, args) => {
                var timespan = _elapsedTime.Elapsed;
                _logger?.LogDebug($"{AppDomain.CurrentDomain.FriendlyName} running time: {timespan:dd} day(s) {timespan:hh}:{timespan:mm}:{timespan:ss}.{timespan:fff}");
                BackgroundTimerTick?.Invoke(timespan);
            };
            monitor.Interval = TimeSpan.FromSeconds(_config.MonitorFrequencySeconds).TotalMilliseconds;
            monitor.Start();
        }
    }
}
