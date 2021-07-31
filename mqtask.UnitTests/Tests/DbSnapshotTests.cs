using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using mqtask.Domain;
using mqtask.Persistence;
using mqtask.UnitTests.Base;
using Xunit;
using Xunit.Abstractions;

namespace mqtask.UnitTests.Tests
{
    public class DbSnapshotTests : BaseUnitTest
    {
        public DbSnapshotTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void TestDuration()
        {
            // 1. Test snapshot generating time
            var snapshotCreator = new DbSnapshotBuilder();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var snapshot = snapshotCreator.Build();
            stopwatch.Stop();

            _testOutputHelper.WriteLine(stopwatch.Elapsed.TotalMilliseconds.ToString());

            Assert.True(stopwatch.Elapsed.TotalMilliseconds < 50);

            // 2. Write ips.scv and cities.csv file for using them in load testing (JMeter)

            // 2.1 Write a list of ips to ips.csv
            Random random = new Random();
            StringBuilder sbIps = new StringBuilder();
            
            for (int i = 0; i < 10000; i++)
                sbIps.AppendLine(IpConverter.ConvertFromIntegerToIpAddress((uint)random.Next(1887073489)));
            
            File.WriteAllText("D://ips.csv", sbIps.ToString());

            // 2.2 Write a list of cities to cities.csv
            StringBuilder sbCities = new StringBuilder();

            var cities = snapshot.Locations.OrderBy(x => x.Organization).Select(x => x.City).Distinct();

            foreach (var city in cities)
                sbCities.AppendLine(city);

            File.WriteAllText("D://cities.csv", sbCities.ToString());

        }
    }
}
