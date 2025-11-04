namespace backend.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public string Flight_Number { get; set; } = string.Empty;

        // foreign keys
        public int Origin_Airport_Id { get; set; }
        public int Destination_Airport_Id { get; set; }

        public DateTimeOffset Departure_Time { get; set; }
    }
}
