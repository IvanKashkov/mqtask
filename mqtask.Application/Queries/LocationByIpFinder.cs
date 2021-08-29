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
            var arr = dbSnapshot.IpRanges;

            IpRange range = BinarySearch(arr, value);

            if (range != null)
            {
                uint locationIndex = dbSnapshot.LocationIndexes[range.LocationIndex];
                Location result = dbSnapshot.Locations[locationIndex];

                LocationInfoUpdater.Update(result, dbSnapshot);

                return result;
            }

            return null;
        }

        private static IpRange BinarySearch(IpRange[] arr, uint value)
        {
            var left = 0;
            var right = arr.Length - 1;
            int index;

            while (left <= right)
            {
                index = left + (right - left) / 2;

                if (arr[index].IpFrom <= value && value <= arr[index].IpTo)
                    return arr[index];

                if (value > arr[index].IpTo)
                    left = index + 1;

                if (value < arr[index].IpFrom)
                    right = index - 1;
            }

            return null;
        }
    }
}
