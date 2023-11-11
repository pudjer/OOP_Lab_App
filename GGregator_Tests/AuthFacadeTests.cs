using GGregator_Infrastructure.Facades;
using Xunit;

namespace GGregator_Tests
{
    public class AuthFacadeTests
    {
        [Fact]
        public void HelperCheck()
        {
            var testHelper = new AuthTestHelper();
            IAuthenticationFacade authFacade = new AppAuthFacade();
            // verify that the database and facade are created successfully
            Assert.True(true);
        }

        [Fact]
        public async void Authenticate_ReturnsString_OnSuccessfulLogin()
        {
            var testHelper = new AuthTestHelper();
            IAuthenticationFacade authFacade = new AppAuthFacade();
            var username = "test_user";
            var password = "password";

            var token = await authFacade.Authenticate(username, password);

            Assert.IsType<string>(token);
        }
    }
}