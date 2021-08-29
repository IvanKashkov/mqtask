using System;
using System.Collections.Generic;

namespace mqtask.Domain.Entities
{
    public class DbSnapshot
    {
        public DbSnapshot(byte[] geoBaseBytes,
            IpRange[] ranges, 
            Location[] locations, 
            uint[] locationIndexes, 
            Dictionary<String, List<Location>> locationsByCityLookup)
        {
            GeoBaseBytes = geoBaseBytes;
            IpRanges = ranges;
            Locations = locations;
            LocationIndexes = locationIndexes;
            LocationsByCityLookup = locationsByCityLookup;
        }

        public IpRange[] IpRanges { get; }
        public Location[] Locations { get; }
        public uint[] LocationIndexes { get; }

        public byte[] GeoBaseBytes { get; }

        // Dictionary is thread safe for read
        public Dictionary<String, List<Location>> LocationsByCityLookup { get; }
    }
}
