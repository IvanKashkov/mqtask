namespace mqtask.Domain.Entities
{
    public class Location
    {
        public static char Zero = '\0';

        public Location(string country, string region, string postal, string city, string organization, float lattitude, float longitude)
        {
            Country = country;
            Region = region;
            Postal = postal;
            City = city;
            Organization = organization;

            Lattitude = lattitude;
            Longitude = longitude;
        }

        public string Country { get; private set; } 
        public string Region { get; private set; } 
        public string Postal { get; private set; } 
        public string City { get; private set; } 
        public string Organization { get; private set; } 

        public float Lattitude { get; } 
        public float Longitude { get; }


        public void TrimZeroSymbol()
        {
            Country = Country.TrimEnd(Zero);
            Region = Region.TrimEnd(Zero);
            Postal = Postal.TrimEnd(Zero);
            City = City.TrimEnd(Zero);
            Organization = Organization.TrimEnd(Zero);
        }
    }
}
