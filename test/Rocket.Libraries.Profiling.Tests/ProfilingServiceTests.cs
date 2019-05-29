using Rocket.Libraries.Profiling.Models;
using Rocket.Libraries.Profiling.Services;
using System;
using Xunit;

namespace Rocket.Libraries.Profiling.Tests
{
    public class ProfilingServiceTests
    {
        [Fact]
        public void TestDuration()
        {
            var durationTestTimeSpan = default(TimeSpan);
            Action<ProfilingInformation> durationSetter = (a) =>
            {
                durationTestTimeSpan = a.Duration;
            };

            var sleepDuration = new TimeSpan(0, 0, 3);
            using (new ProfilerService(a => durationSetter(a), "test", false))
            {
                System.Threading.Thread.Sleep(sleepDuration);
            }
            var durationGreaterThanSleep = durationTestTimeSpan > sleepDuration;
            Assert.True(durationGreaterThanSleep);
        }
    }
}