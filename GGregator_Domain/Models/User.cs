namespace GGregator_Domain.Models
{
    public partial class User : TimestampedModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
