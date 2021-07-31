using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace mqtask.Domain.Entities
{
    public class DbSnapshot
    {
        private static char Zero = '\0';

        public DbSnapshot(IpRange[] ranges, Location[] locations, int[] locationIndexes)
        {
            IpRanges = ranges;
            Locations = locations;
            LocationIndexes = locationIndexes;
        }

        public IpRange[] IpRanges { get; private set; }
        public Location[] Locations { get; private set; }
        public string[] LocationsJson { get; private set; }
        public int[] LocationIndexes { get; private set; }


        private Dictionary<String, string> _locationsByCityDictionary;

        // Dictionary is thread safe for read
        public Dictionary<String, string> LocationsByCityDictionary => _locationsByCityDictionary;

        /// <summary>
        /// Do some additional logic that we need to do after the file is read.
        /// </summary>
        public void Prepare()
        {
            // 1. Trim all of location data for ending zero symbol
            foreach (var location in Locations)
                location.TrimZeroSymbol();

            // 2. Prepare location serialized info. We will use like index
            LocationsJson = Locations.Select(x => JsonSerializer.Serialize(x)).ToArray();

            // 3. Prepare locations by city serialized info.
            _locationsByCityDictionary = Locations.GroupBy(x => x.City).ToDictionary(x => x.Key, x => JsonSerializer.Serialize(x.ToArray()));
        }
    }
}
