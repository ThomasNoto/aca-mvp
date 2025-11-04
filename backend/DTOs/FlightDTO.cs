public class FlightDTO
{
    public int Id { get; set; }
    public string Flight_Number { get; set; } = string.Empty;
    public string Origin_Iata { get; set; } = string.Empty;
    public string Destination_Iata { get; set; } = string.Empty;
    public DateTimeOffset Departure_Time { get; set; }
}
