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
                var country = Encoding.ASCII.GetString(dbSnapshot.GeoBaseBytes, result.OriginalByteArrIndex,
                    AsciiZeroCharacterSearcher.IndexOf(new ReadOnlySpan<byte>(dbSnapshot.GeoBaseBytes, result.OriginalByteArrIndex, 8)));

                var region = Encoding.ASCII.GetString(dbSnapshot.GeoBaseBytes, result.OriginalByteArrIndex,
                    AsciiZeroCharacterSearcher.IndexOf(new ReadOnlySpan<byte>(dbSnapshot.GeoBaseBytes, result.OriginalByteArrIndex + 8, 12)));

                var postal = Encoding.ASCII.GetString(dbSnapshot.GeoBaseBytes, result.OriginalByteArrIndex,
                    AsciiZeroCharacterSearcher.IndexOf(new ReadOnlySpan<byte>(dbSnapshot.GeoBaseBytes, result.OriginalByteArrIndex + 20, 12)));

                var organization = Encoding.ASCII.GetString(dbSnapshot.GeoBaseBytes, result.OriginalByteArrIndex,
                    AsciiZeroCharacterSearcher.IndexOf(new ReadOnlySpan<byte>(dbSnapshot.GeoBaseBytes, result.OriginalByteArrIndex + 56, 32)));

                var latitude = BitConverter.ToSingle(new ReadOnlySpan<byte>(dbSnapshot.GeoBaseBytes, result.OriginalByteArrIndex + 60, 4));
                
                var longitude = BitConverter.ToSingle(new ReadOnlySpan<byte>(dbSnapshot.GeoBaseBytes, result.OriginalByteArrIndex + 64, 4));

                result.Update(country, region, postal, organization, latitude, longitude);
            }
        }
    }
}
