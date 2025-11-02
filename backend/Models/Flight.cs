namespace backend.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public string Flight_Number { get; set; } = string.Empty;

        // foreign keys
        public int Origin_Airport_Id { get; set; }
        public int Destination_Airport_Id { get; set; }

        // airport object associated with flight for ef core relationship
        public Airport OriginAirport { get; set; } = null!;
        public Airport DestinationAirport { get; set; } = null!;

        public DateTimeOffset Departure_Time { get; set; }
        public DateTimeOffset Arrival_Time { get; set; }
        public string Aircraft_Type { get; set; } = string.Empty;
        public FlightStatus Status { get; set; }
    }
}
