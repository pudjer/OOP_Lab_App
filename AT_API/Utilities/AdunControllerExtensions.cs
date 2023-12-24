using AT_Domain.Models;
using AT_Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AT_API.Utilities
{
    internal static class AdunControllerExtensions
    {
        public static async Task<User?> GetUserAsync(this ControllerBase controller)
        {
            var id = controller.User.FindFirstValue("id");
            if (id == null)
            {
                return null;
            }
            var userRepository = controller.HttpContext.RequestServices
                .GetService<IBaseModelRepository<User>>();
            User? currentUser = await userRepository.GetAsync(new Guid(id));
            return currentUser;
        }
    }
}
