namespace backend.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}
