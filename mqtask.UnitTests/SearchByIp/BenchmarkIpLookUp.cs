using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using mqtask.Application.Queries;
using mqtask.Persistence;
using mqtask.UnitTests.Base;
using mqtask.UnitTests.Current;
using Xunit;
using Xunit.Abstractions;

namespace mqtask.UnitTests.SearchByIp
{
    [SimpleJob(RuntimeMoniker.Net50, targetCount: 10)]
    [RPlotExporter]
    public class BenchmarkByIpLookup
    {
        private Domain.Entities.DbSnapshot dbSnapshot;
        private string[] ips;

        public BenchmarkByIpLookup()
        {
            dbSnapshot = new DbSnapshotBuilder().Build();
            ips = File.ReadAllLines("D:\\ips.csv");
        }

        [Benchmark]
        public void StructTest()
        {
            for (int j = 0; j < 5; j++)
                for (int i = 0; i < 10000; i++)
                    LocationByIpFinder.Find(dbSnapshot, ips[i]);
        }
    }

    public class BenchmarkByIpLookupTests : BaseUnitTest
    {
        public BenchmarkByIpLookupTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void DbSnapshotBenchmarkTest()
        {
            BenchmarkRunner.Run<BenchmarkByIpLookup>();
        }
    }
}
