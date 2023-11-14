using GGregator_Domain.DTOs.OutDTOs;

namespace GGregator_Infrastructure.Facades
{
    public interface IAuthenticationFacade
    {
        public Task<SignedUpDTO?> Register(string username, string password);

        public Task<LoggedInDTO?> Authenticate(string username, string password);
    }
}
