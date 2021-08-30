using System;
using System.Collections.Generic;

namespace mqtask.Domain.Entities
{
    public class DbSnapshot
    {
        public DbSnapshot(byte[] geoBaseBytes,
            IpRange[] ranges, 
            Location[] locations, 
            uint[] locationIndexes)
        {
            GeoBaseBytes = geoBaseBytes;
            IpRanges = ranges;
            Locations = locations;
            LocationIndexes = locationIndexes;
        }

        public IpRange[] IpRanges { get; }
        public Location[] Locations { get; }
        public uint[] LocationIndexes { get; }

        public byte[] GeoBaseBytes { get; }
        
        public Dictionary<String, List<Location>> LocationsByCityLookup { get; set; }
    }
}
