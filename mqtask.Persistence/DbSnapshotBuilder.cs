using System;
using System.IO;
using System.Linq;
using System.Text;
using mqtask.Domain.Entities;
using mqtask.Domain.Services;
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
            int index = 44;
            int records = ReadInt32(bytes, index);
            index += 4;
            int offset_ranges = DbSnapshotBuilder.ReadInt32(bytes, index);
            index += 4;
            int offset_cities = DbSnapshotBuilder.ReadInt32(bytes, index);
            index += 4;
            int offset_locations = DbSnapshotBuilder.ReadInt32(bytes, index);
            index += 4;

            var ranges = new IpRange[records];
            var locations = new Location[records];
            var locationIndexes = new uint[records];

            for (uint i = 0; i < records; i++)
            {
                locationIndexes[i] = ReadUInt32(bytes, offset_cities) / 96;
                offset_cities += 4;
            }

            // 3. Read IpRanges
            for (uint i = 0; i < records; i++)
            {
                var from = ReadUInt32(bytes, index); index += 4;
                var to = ReadUInt32(bytes, index); index += 4;
                var locationIndex = ReadUInt32(bytes, index); index += 4;

                var finalIndex = locationIndexes[locationIndex];

                ranges[i] = new IpRange(from, to, finalIndex);
            }

            // 4. Read Locations
            for (uint i = 0; i < records; i++)
            {
                int byteIndex = index;
                index += 32;
                var city = ReadString(bytes, index, AsciiZeroCharacterSearcher.IndexOf(new ReadOnlySpan<byte>(bytes, index, 24)));
                index += 64;
                locations[i] = new Location(byteIndex, city);
            }

            // 5. Read Location indexes.

            // 6. Return DbSnapshot
            var result = new DbSnapshot(bytes, ranges, locations);

            return result;
        }

        public static int ReadInt32(byte[] bytes, int index)
        {
            return BitConverter.ToInt32(new ReadOnlySpan<byte>(bytes, index, 4));
        }

        public static uint ReadUInt32(byte[] bytes, int index)
        {
            return BitConverter.ToUInt32(new ReadOnlySpan<byte>(bytes, index, 4));
        }

        public static string ReadString(byte[] bytes, int index, int length)
        {
            return Encoding.ASCII.GetString(bytes, index, length);
        }

        public static ulong ReadUInt64(byte[] bytes, int index)
        {
            return BitConverter.ToUInt64(new ReadOnlySpan<byte>(bytes, index, 8));
        }
    }
}
