using AT_Domain.Models;
using AT_Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace AT_API.Utilities
{
    /// <summary>
    /// <para>Обработчик авторизации для работы непосредственно с базой данных
    /// (получает данные о ролях и правах доступа пользователя не из токена,
    /// а непосредственно из записей в БД).</para>
    /// </summary>
    public sealed class AdunDatabaseAuthorizationHandler :
        AuthorizationHandler<RolesAuthorizationRequirement>,
        IAuthorizationHandler
    {
        public const string ADMIN_ROLE = "Admin";
        public const string SUBSCRIBED_ROLE = "Subscribed";

        private readonly IBaseModelRepository<User> _userRepository;
        public AdunDatabaseAuthorizationHandler(IBaseModelRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            RolesAuthorizationRequirement requirement)
        {
            if (context.User == null)
            {
                context.Fail();
                return;
            }
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return;
            }

            var idClaim = context.User.FindFirst("id");
            if (idClaim == null)
            {
                context.Fail();
                return;
            }
            var user = await _userRepository.GetAsync(new Guid(idClaim.Value));
            if (user == null)
            {
                context.Fail();
                return;
            }
            
            if (requirement.AllowedRoles == null ||
                requirement.AllowedRoles.Any() == false)
            {
                context.Succeed(requirement);
                return;
            }
            else
            {
                var roles = requirement.AllowedRoles;
                if (roles.Contains(ADMIN_ROLE) && user.IsAdmin ||
                    roles.Contains(SUBSCRIBED_ROLE) &&
                        (user.IsAdmin || user.IsSubscribed)
                    )
                {
                    context.Succeed(requirement);
                    return;
                }
            }
            context.Fail();
        }
    }
}
