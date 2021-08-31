using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using mqtask.Application.Queries;
using mqtask.Persistence;
using mqtask.UnitTests.Base;
using Xunit;
using Xunit.Abstractions;

namespace mqtask.UnitTests.DbSnapshot
{
    [SimpleJob(RuntimeMoniker.Net50, targetCount: 10)]
    [RPlotExporter]
    public class BenchmarkIpLookup
    {
        private Domain.Entities.DbSnapshot dbSnapshot;

        public BenchmarkIpLookup()
        {
            dbSnapshot = new DbSnapshotBuilder().Build();
        }

        [Benchmark]
        public void Test()
        {
            LocationByIpFinder.Find(dbSnapshot, "85.145.154.168");
        }
    }

    public class BenchmarkIpLookupTests : BaseUnitTest
    {
        public BenchmarkIpLookupTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void DbSnapshotBenchmarkTest()
        {
            BenchmarkRunner.Run<BenchmarkIpLookup>();
        }
    }
}
