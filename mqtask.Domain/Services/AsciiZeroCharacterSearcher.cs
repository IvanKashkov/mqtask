using System;

namespace mqtask.Domain.Services
{
    public class AsciiZeroCharacterSearcher
    {
        public static int IndexOf(ReadOnlySpan<byte> span)
        {
            var left = 0;
            var right = span.Length - 1;
            int firstZero = 0;

            while (left <= right)
            {
                var index = (right + left) / 2;

                if (span[index] == 0)
                {
                    right = index - 1;
                    firstZero = index;
                }
                else
                    left = index + 1;
            }

            return firstZero;
        }
    }
}
