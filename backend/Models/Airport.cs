namespace backend.Models
{
    public class Airport
    {
        public int Id { get; set; }
        public string Iata_Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
    }
}

