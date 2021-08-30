using System;
using System.Text;
using mqtask.Domain.Entities;
using mqtask.Domain.Services;

namespace mqtask.Application.Commands
{
    public class LocationInfoUpdater
    {
        public static void Update(Location result, DbSnapshot dbSnapshot)
        {
            if (string.IsNullOrEmpty(result.Country))
            {
                int index = result.OriginalByteArrIndex;

                var country = Encoding.ASCII.GetString(dbSnapshot.GeoBaseBytes, index, AsciiZeroCharacterSearcher.IndexOf(new ReadOnlySpan<byte>(dbSnapshot.GeoBaseBytes, index, 8)));
                index += 8;
                var region = Encoding.ASCII.GetString(dbSnapshot.GeoBaseBytes, index, AsciiZeroCharacterSearcher.IndexOf(new ReadOnlySpan<byte>(dbSnapshot.GeoBaseBytes, index, 12)));
                index += 12;
                var postal = Encoding.ASCII.GetString(dbSnapshot.GeoBaseBytes, index, AsciiZeroCharacterSearcher.IndexOf(new ReadOnlySpan<byte>(dbSnapshot.GeoBaseBytes, index, 12)));
                index += 36; // 12 + 24 (City)
                var organization = Encoding.ASCII.GetString(dbSnapshot.GeoBaseBytes, index, AsciiZeroCharacterSearcher.IndexOf(new ReadOnlySpan<byte>(dbSnapshot.GeoBaseBytes, index, 32)));
                index += 32;
                var latitude = BitConverter.ToSingle(new ReadOnlySpan<byte>(dbSnapshot.GeoBaseBytes, index, 4));
                index += 4;
                var longitude = BitConverter.ToSingle(new ReadOnlySpan<byte>(dbSnapshot.GeoBaseBytes, index, 4));

                result.Update(country, region, postal, organization, latitude, longitude);
            }
        }
    }
}
