using System;
using System.Collections.Generic;
using System.Linq;
using mqtask.Domain.Entities;
using LocationInfoUpdater = mqtask.Application.Commands.LocationInfoUpdater;

namespace mqtask.Application.Queries
{
    public static class LocationsByCityFinder
    {
        private static Dictionary<String, List<Location>> LocationsByCityLookup { get; set; }
        private static readonly object SynchObj = new object();

        public static List<Location> Find(DbSnapshot dbSnapshot, string city)
        {
            if (LocationsByCityLookup == null)
            {
                lock (SynchObj)
                {
                    if (LocationsByCityLookup == null)
                    {
                        LocationsByCityLookup = dbSnapshot.Locations.GroupBy(x => x.City).ToDictionary(x => x.Key, x => x.ToList());
                    }
                }
            }

            var result = LocationsByCityLookup.ContainsKey(city) ? LocationsByCityLookup[city] : null;

            if (result != null)
                result.ForEach(location => LocationInfoUpdater.Update(location, dbSnapshot));

            return result;
        }
    }
}
