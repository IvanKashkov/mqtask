using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace mqtask.Domain.Entities
{
    [StructLayout(LayoutKind.Sequential)]
    public struct IpRange
    {
        public IpRange(uint ipFrom, uint ipTo, uint locationIndex)
        {
            IpFrom = ipFrom;
            IpTo = ipTo;
            LocationIndex = locationIndex;
        }

        public uint IpFrom { get; }
        public uint IpTo { get; }
        public uint LocationIndex { get; }
    }
}
