using System;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using mqtask.UnitTests.Base;
using Xunit;
using Xunit.Abstractions;

namespace mqtask.UnitTests.Investigation
{
    public class ParseStringBenchmarkTest : BaseUnitTest
    {
        public ParseStringBenchmarkTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void TestParsingApproaches()
        {
            BenchmarkRunner.Run<ParseStringBenchmark>();
        }
    }


    [SimpleJob(RuntimeMoniker.Net50)]
    [RPlotExporter]
    public class ParseStringBenchmark
    {
        private byte[] bytes;

        [Params(1000)]
        public int N;

        [Params(10, 20, 30)]
        public int Count;

        public ParseStringBenchmark()
        {
            bytes = new byte[] { 
                246, 32, 70, 200, 60, 55, 7, 15, 
                251, 230, 100, 70, 40, 199, 46, 
                100, 30, 20, 170, 1, 17, 56, 32, 
                77, 88, 60, 55, 7, 15, 251, 230,
                246, 32, 70, 200, 60, 55, 7, 15,
                251, 230, 100, 70, 40, 199, 46,
                100, 30, 20, 170, 1, 17, 56, 32,
                77, 88, 60, 55, 7, 15, 251, 230,
                246, 32, 70, 200, 60, 55, 7, 15,
                251, 230, 100, 70, 40, 199, 46,
                100, 30, 20, 170, 1, 17, 56, 32,
                77, 88, 60, 55, 7, 15, 251, 230,
                246, 32, 70, 200, 60, 55, 7, 15,
                251, 230, 100, 70, 40, 199, 46,
                100, 30, 20, 170, 1, 17, 56, 32,
                77, 88, 60, 55, 7, 15, 251, 230,
                246, 32, 70, 200, 60, 55, 7, 15,
                251, 230, 100, 70, 40, 199, 46,
                100, 30, 20, 170, 1, 17, 56, 32,
                77, 88, 60, 55, 7, 15, 251, 230
            };
        }

        [Benchmark]
        public void AsciiGetString()
        {
            for (int i = 0; i < N; i++)
            {
                Encoding.ASCII.GetString(bytes, 0, Count);
            }
        }

        [Benchmark]
        public void StringBuilderAndCastingToChar()
        {
            for (int i = 0; i < N; i++)
            {
                StringBuilder builder = new StringBuilder();
                var span = new ReadOnlySpan<byte>(bytes, 0, Count);

                for (int j = 0; j < span.Length; j++)
                {
                    builder.Append((char)span[j]);
                }

                var result = builder.ToString();
            }
        }

        [Benchmark]
        public void BitConverterToString()
        {
            for (int i = 0; i < N; i++)
            {
                BitConverter.ToString(bytes, 0, Count);
            }
        }
    }
}
