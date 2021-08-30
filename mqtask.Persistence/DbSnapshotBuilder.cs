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
            index += 16;

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
            var locationIndexes = new uint[records];

            for (uint i = 0; i < records; i++)
            {
                locationIndexes[i] = ReadUInt32(bytes, index) / 96;
                index += 4;
            }

            // 6. Return DbSnapshot
            var result = new DbSnapshot(bytes, ranges, locations, locationIndexes);

            return result;
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
            return Encoding.ASCII.GetString(bytes, index, length);
        }

        private ulong ReadUInt64(byte[] bytes, int index)
        {
            return BitConverter.ToUInt64(new ReadOnlySpan<byte>(bytes, index, 8));
        }
    }
}
