using System;
using System.Collections.Generic;

namespace mqtask.Domain.Entities
{
    public class DbSnapshot
    {
        public DbSnapshot(byte[] geoBaseBytes,
            IpRange[] ranges, 
            Location[] locations)
        {
            GeoBaseBytes = geoBaseBytes;
            IpRanges = ranges;
            Locations = locations;
        }

        public IpRange[] IpRanges { get; }
        public Location[] Locations { get; }
        public byte[] GeoBaseBytes { get; }

        public Dictionary<String, List<Location>> LocationsByCityLookup { get; set; }
    }
}
