using System;
using System.Net;

namespace mqtask.Domain.Services
{
    public static class IpConverter
    {
        public static uint ConvertFromIpAddressToInteger(string ipAddress)
        {
            var address = IPAddress.Parse(ipAddress);
            byte[] bytes = address.GetAddressBytes();
            return BitConverter.ToUInt32(bytes, 0);
        }

        public static string ConvertFromIntegerToIpAddress(uint ipAddress)
        {
            byte[] bytes = BitConverter.GetBytes(ipAddress);
            return new IPAddress(bytes).ToString();
        }
    }
}
