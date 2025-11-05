using System.ComponentModel.DataAnnotations;

public class FlightCreateDTO
{
    [Required]
    public string Flight_Number { get; set; } = string.Empty;

    [Required]
    public int Origin_Airport_Id { get; set; }

    [Required]
    public int Destination_Airport_Id { get; set; }

    [Required]
    public DateTimeOffset Departure_Time { get; set; }
}