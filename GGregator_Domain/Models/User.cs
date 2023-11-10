namespace GGregator_Domain.Models
{
    public partial class User
    {
        public enum UserRole
        {
            Standard,
            Admin,
        }
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }

    }
}
