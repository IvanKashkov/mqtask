using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using mqtask.Persistence;
using mqtask.UnitTests.Base;
using Xunit;
using Xunit.Abstractions;

namespace mqtask.UnitTests.DbSnapshot
{
    [SimpleJob(RuntimeMoniker.Net50, targetCount: 15)]
    [RPlotExporter]
    public class Benchmark
    {
        public Benchmark()
        {
        }

        [Benchmark]
        public void AsciiGetString()
        {
            var snapshotCreator = new DbSnapshotBuilder();
            snapshotCreator.Build();
        }
    }

    public class BenchmarkTests : BaseUnitTest
    {
        public BenchmarkTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void DbSnapshotBenchmarkTest()
        {
            BenchmarkRunner.Run<Benchmark>();
        }
    }
}
