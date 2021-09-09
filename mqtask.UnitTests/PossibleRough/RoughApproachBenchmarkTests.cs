using System;
using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using mqtask.Domain.Services;
using mqtask.Persistence;
using mqtask.UnitTests.Base;
using Xunit;
using Xunit.Abstractions;

namespace mqtask.UnitTests.PossibleRough
{
    [SimpleJob(RuntimeMoniker.Net50, targetCount: 10)]
    [RPlotExporter]
    public class UseJustInMemoryBytesBenchmark
    {
        private Byte[] bytes;

        public UseJustInMemoryBytesBenchmark()
        {
            bytes = File.ReadAllBytes("geobase.dat");
        }

        public void Do()
        {
            uint ip = IpConverter.ConvertFromIpAddressToInteger("85.145.154.168");

            int index = 44;
            int records = DbSnapshotBuilder.ReadInt32(bytes, index);
            index += 4;
            int offset_ranges = DbSnapshotBuilder.ReadInt32(bytes, index);
            index += 4;
            int offset_cities = DbSnapshotBuilder.ReadInt32(bytes, index);
            index += 4;
            int offset_locations = DbSnapshotBuilder.ReadInt32(bytes, index);

            int locationIndex = BinarySearch(bytes, offset_ranges, 0, records - 1, ip);
            int cityIndex = DbSnapshotBuilder.ReadInt32(bytes, offset_cities + locationIndex * 4);

            int cityInfoIndex = offset_locations + cityIndex;

            var country = Encoding.ASCII.GetString(bytes, cityInfoIndex, AsciiZeroCharacterSearcher.IndexOf(new ReadOnlySpan<byte>(bytes, cityInfoIndex, 8)));
            cityInfoIndex += 8;
            var region = Encoding.ASCII.GetString(bytes, cityInfoIndex, AsciiZeroCharacterSearcher.IndexOf(new ReadOnlySpan<byte>(bytes, cityInfoIndex, 12)));
            cityInfoIndex += 12;
            var postal = Encoding.ASCII.GetString(bytes, cityInfoIndex, AsciiZeroCharacterSearcher.IndexOf(new ReadOnlySpan<byte>(bytes, cityInfoIndex, 12)));
            cityInfoIndex += 12;
            var city = Encoding.ASCII.GetString(bytes, cityInfoIndex, AsciiZeroCharacterSearcher.IndexOf(new ReadOnlySpan<byte>(bytes, cityInfoIndex, 24)));
            cityInfoIndex += 24;
            var organization = Encoding.ASCII.GetString(bytes, cityInfoIndex, AsciiZeroCharacterSearcher.IndexOf(new ReadOnlySpan<byte>(bytes, cityInfoIndex, 32)));
            cityInfoIndex += 32;
            var latitude = BitConverter.ToSingle(new ReadOnlySpan<byte>(bytes, cityInfoIndex, 4));
            cityInfoIndex += 4;
            var longitude = BitConverter.ToSingle(new ReadOnlySpan<byte>(bytes, cityInfoIndex, 4));
        }

        [Benchmark]
        public void SecondApproach()
        {
            Do();
        }

        private static int BinarySearch(byte[] bytes, int rangeStartIndex, int left, int right, uint value)
        {
            int index;

            while (left <= right)
            {
                index = left + (right - left) / 2;
                int byteIndex = rangeStartIndex + 12 * (index + 1);

                var ipFrom = DbSnapshotBuilder.ReadUInt32(bytes, byteIndex);
                byteIndex += 4;
                var ipTo = DbSnapshotBuilder.ReadUInt32(bytes, byteIndex);
                byteIndex += 4;

                if (ipFrom <= value && value <= ipTo)
                    return DbSnapshotBuilder.ReadInt32(bytes, byteIndex);

                if (value > ipTo)
                    left = index + 1;

                if (value < ipFrom)
                    right = index - 1;
            }

            return 0;
        }
    }

    public class RoughApproachBenchmarkTests : BaseUnitTest
    {
        public RoughApproachBenchmarkTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void DbSnapshotBenchmarkTest()
        {
            BenchmarkRunner.Run<UseJustInMemoryBytesBenchmark>();
        }

        [Fact]
        public void Approach()
        {
            new UseJustInMemoryBytesBenchmark().Do();
        }

        
    }
}
