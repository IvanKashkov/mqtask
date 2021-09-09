using mqtask.Domain.Entities;
using mqtask.Domain.Services;
using LocationInfoUpdater = mqtask.Application.Commands.LocationInfoUpdater;

namespace mqtask.Application.Queries
{
    public class LocationByIpFinder
    {
        public static Location Find(DbSnapshot dbSnapshot, string ip)
        {
            var value = IpConverter.ConvertFromIpAddressToInteger(ip);

            IpRange? range = BinarySearch(dbSnapshot.IpRanges, value);
            
            if (range != null)
            {
                Location result = dbSnapshot.Locations[range.Value.LocationIndex];
                LocationInfoUpdater.Update(result, dbSnapshot);

                return result;
            }

            return null;
        }

        private static IpRange? BinarySearch(IpRange[] arr, uint value)
        {
            var left = 0;
            var right = arr.Length - 1;
            int index;

            while (left <= right)
            {
                index = (right + left) / 2;

                if (value > arr[index].IpTo)
                    left = index + 1;
                else if (value < arr[index].IpFrom)
                    right = index - 1;
                else
                    return arr[index];
            }

            return null;
        }
    }
}
