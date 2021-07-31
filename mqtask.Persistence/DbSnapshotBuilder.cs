using System;
using System.IO;
using System.Text;
using mqtask.Domain.Entities;
using mqtask.Persistence.Interfaces;

namespace mqtask.Persistence
{
    /// <summary>
    /// Building in memory snapshot by reading geobase.dat file.
    /// </summary>
    public class DbSnapshotBuilder : IDbSnapshotBuilder
    {
        public DbSnapshot Build()
        {
            // 1. Read all bytes at once.
            Byte[] bytes = File.ReadAllBytes("geobase.dat");

            // 2. Read meta information
            int index = 0;
            index += 44;
            var records = ReadUInt32(bytes, index); index += 16;

            var ranges = new IpRange[records];
            var locations = new Location[records];

            // 3. Read IpRanges
            for (uint i = 0; i < records; i++)
            {
                var from = ReadUInt32(bytes, index); index += 4;
                var to = ReadUInt32(bytes, index); index += 4;
                var locationIndex = ReadUInt32(bytes, index); index += 4;

                ranges[i] = new IpRange(from, to, locationIndex);
            }

            // 4. Read locations
            for (uint i = 0; i < records; i++)
            {
                var country = ReadString(bytes, index, 8); index += 8;
                var region = ReadString(bytes, index, 12); index += 12;
                var postal = ReadString(bytes, index, 12); index += 12;
                var city = ReadString(bytes, index, 24); index += 24;
                var organization = ReadString(bytes, index, 32); index += 32;

                var latitude = BitConverter.ToSingle(new ReadOnlySpan<byte>(bytes, index, 4)); index += 4;
                var longitude = BitConverter.ToSingle(new ReadOnlySpan<byte>(bytes, index, 4)); index += 4;

                locations[i] = new Location(country, region, postal, city, organization, latitude, longitude);
            }

            // 5. Read location indexes.
            var locationIndexes = new int[records];

            for (uint i = 0; i < records; i++)
            {
                locationIndexes[i] = ReadInt32(bytes, index) / 96; index += 4;
            }

            // 6. Return DbSnapshot
            return new DbSnapshot(ranges, locations, locationIndexes);
        }

        private int ReadInt32(byte[] bytes, int index)
        {
            return BitConverter.ToInt32(new ReadOnlySpan<byte>(bytes, index, 4));
        }

        private uint ReadUInt32(byte[] bytes, int index)
        {
            return BitConverter.ToUInt32(new ReadOnlySpan<byte>(bytes, index, 4));
        }

        private string ReadString(byte[] bytes, int index, int length)
        {

            // We have file in ASCII format. And Encoding.ASCII.GetString() works faster than BitConverter.ToString()
            return Encoding.ASCII.GetString(bytes, index, length);
        }

        private ulong ReadUInt64(byte[] bytes, int index)
        {
            return BitConverter.ToUInt64(new ReadOnlySpan<byte>(bytes, index, 8));
        }
    }
}
