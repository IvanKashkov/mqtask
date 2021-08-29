using System.Diagnostics;
using mqtask.Persistence;
using mqtask.UnitTests.Base;
using Xunit;
using Xunit.Abstractions;

namespace mqtask.UnitTests.DbSnapshot
{
    public class DurationTests : BaseUnitTest
    {
        public DurationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void TestDuration()
        {
            var snapshotCreator = new DbSnapshotBuilder();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var snapshot = snapshotCreator.Build();
            stopwatch.Stop();

            _testOutputHelper.WriteLine(stopwatch.Elapsed.TotalMilliseconds.ToString());

            Assert.True(stopwatch.Elapsed.TotalMilliseconds < 50);
        }
    }
}
