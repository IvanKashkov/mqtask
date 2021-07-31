using mqtask.Domain.Entities;

namespace mqtask.Domain
{
    public static class LocationsByCityFinder
    {
        public static string Find(DbSnapshot db, string city)
        {
            return db.LocationsByCityDictionary.ContainsKey(city) ? db.LocationsByCityDictionary[city] : null;
        }
    }
}
