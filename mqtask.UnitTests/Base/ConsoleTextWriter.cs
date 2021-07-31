using System.IO;
using System.Text;
using Xunit.Abstractions;

namespace mqtask.UnitTests.Base
{
    public class ConsoleTextWriter : TextWriter
    {
        private ITestOutputHelper _output;

        public ConsoleTextWriter(ITestOutputHelper output)
        {
            _output = output;
        }
        public override Encoding Encoding => Encoding.UTF8;

        public override void WriteLine(string message)
        {
            _output.WriteLine(message);
        }
        public override void WriteLine(string format, params object[] args)
        {
            _output.WriteLine(format, args);
        }

        public override void WriteLine(string format, object? arg0)
        {
            _output.WriteLine(format, arg0);
        }

        public override void WriteLine(StringBuilder? value)
        {
            if (value != null)
                _output.WriteLine(value.ToString());
        }

        public override void WriteLine(string format, object? arg0, object? arg1)
        {
            _output.WriteLine(format, arg0, arg1);
        }

        public override void WriteLine(string format, object? arg0, object? arg1, object? arg2)
        {
            _output.WriteLine(format, arg0, arg1, arg2);
        }
    }
}