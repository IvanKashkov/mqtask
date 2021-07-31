using System;
using Xunit.Abstractions;

namespace mqtask.UnitTests.Base
{
    public class BaseUnitTest
    {
        protected readonly ITestOutputHelper _testOutputHelper;

        public BaseUnitTest(ITestOutputHelper testOutputHelper)
        {
            // for using logs in unit tests
            _testOutputHelper = testOutputHelper;

            // for using BenchmarkDotNet logs in unit tests 
            var converter = new ConsoleTextWriter(_testOutputHelper);
            Console.SetOut(converter);
        }
    }
}
