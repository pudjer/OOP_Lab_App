namespace GGregator_Domain.DTOs
{
    public class SignedUpDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
    }
}
