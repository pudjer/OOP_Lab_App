using AT_Domain.DTOs.OutDTOs;

namespace AT_Infrastructure.Facades
{
    public interface IAuthenticationFacade
    {
        public Task<SignedUpDTO?> Register(string username, string password);

        public Task<LoggedInDTO?> Authenticate(string username, string password);
    }
}
