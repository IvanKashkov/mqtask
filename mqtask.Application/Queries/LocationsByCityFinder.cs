using System.Collections.Generic;
using mqtask.Domain.Entities;
using mqtask.Domain.Services;
using LocationInfoUpdater = mqtask.Application.Commands.LocationInfoUpdater;

namespace mqtask.Application.Queries
{
    public static class LocationsByCityFinder
    {
        public static List<Location> Find(DbSnapshot dbSnapshot, string city)
        {
            var result = dbSnapshot.LocationsByCityLookup.ContainsKey(city) ? dbSnapshot.LocationsByCityLookup[city] : null;

            if (result != null)
                result.ForEach(location => LocationInfoUpdater.Update(location, dbSnapshot));

            return result;
        }
    }
}
