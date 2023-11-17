namespace AT_Domain.Models
{
    public partial class User : TimestampedModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; } = false;
        public bool IsSubscribed { get; set; } = false;
    }
}
