using mqtask.Domain.Entities;

namespace mqtask.Domain
{
    public class LocationByIpFinder
    {
        public static string Find(DbSnapshot dbSnapshot, string ip)
        {
            var value = IpConverter.ConvertFromIpAddressToInteger(ip);
            var arr = dbSnapshot.IpRanges;

            IpRange range = BinarySearch(arr, value);

            if (range != null)
            {
                return dbSnapshot.LocationsJson[dbSnapshot.LocationIndexes[range.LocationIndex]];
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
