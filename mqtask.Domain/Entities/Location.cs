namespace mqtask.Domain.Entities
{
    public record Location
    {
        public Location(int originalByteArrIndex, string city)
        {
            OriginalByteArrIndex = originalByteArrIndex;
            City = city;
        }

        /// <summary>
        /// Index in the byte array
        /// </summary>
        public int OriginalByteArrIndex { get; }
        public string City { get; }

        public string Country { get; private set; }
        public string Region { get; private set; }
        public string Postal { get; private set; }
        public string Organization { get; private set; }

        public float Latitude { get; private set; }
        public float Longitude { get; private set; }

        public void Update(string country, string region, string postal, string organization, float latitude, float longitude)
        {
            Country = country;
            Region = region;
            Postal = postal;
            Organization = organization;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
