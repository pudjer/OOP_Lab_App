using BCrypt.Net;
using GGregator_Domain.DTOs;
using GGregator_Infrastructure.Facades;
using Xunit;

namespace GGregator_Tests
{
    // probably gotta use mocks or something later ig
    public class AuthFacadeTests
    {
        [Fact]
        public void HelperCheck()
        {
            var testHelper = new AuthTestHelper();
            IAuthenticationFacade authFacade = testHelper.AuthFacade;
            // verify that the database and facade are created successfully
            Assert.True(true);
        }

        [Fact]
        public async void Register_ReturnsUserDTO_OnSuccessfulSignup()
        {
            var testHelper = new AuthTestHelper();
            IAuthenticationFacade facade = testHelper.AuthFacade;
            string username = "new_user";
            string password = "password";

            var dto = await facade.Register(username, password);

            Assert.IsType<SignedUpDTO>(dto);
        }

        [Theory]
        [InlineData("test_user", "password_for_already_existing_user")]
        [InlineData("nonexistent_user", "password")]
        [InlineData("empty_password", "")]
        [InlineData("", "empty_username")]
        public async void Register_ReturnsNull_OnInvalidSignup(string username, string password)
        {
            var testHelper = new AuthTestHelper();
            IAuthenticationFacade facade = testHelper.AuthFacade;

            var dto = await facade.Register(username, password);

            Assert.Null(dto);
        }

        public async void Register_CreatesUser_OnSuccessfulSignup()
        {
            var testHelper = new AuthTestHelper();
            IAuthenticationFacade facade = testHelper.AuthFacade;
            var username = "new_user";
            var password = "password";

            var dto = await facade.Register(username, password);
            var user = testHelper.Context.Users.Find(dto.Id);

            Assert.NotNull(user);
        }

        public async void Register_PasswordMatches_OnSuccessfulSignup()
        {
            var testHelper = new AuthTestHelper();
            IAuthenticationFacade facade = testHelper.AuthFacade;
            var username = "new_user";
            var password = "password";

            var dto = await facade.Register(username, password);
            var user = testHelper.Context.Users.Find(dto.Id);

            Assert.True(BCrypt.Net.BCrypt.Verify(password, user.Password));
        }

        [Fact]
        public async void Authenticate_ReturnsString_OnSuccessfulLogin()
        {
            var testHelper = new AuthTestHelper();
            IAuthenticationFacade authFacade = testHelper.AuthFacade;
            var username = "test_user";
            var password = "password";

            var token = await authFacade.Authenticate(username, password);

            Assert.IsType<string>(token);
        }

        [Theory]
        [InlineData("test_user", "invalid_password")]
        [InlineData("nonexistent_user", "password")]
        public async void Authenticate_ReturnsNull_OnInvaliLogin(string username, string password)
        {
            var testHelper = new AuthTestHelper();
            IAuthenticationFacade authFacade = testHelper.AuthFacade;

            var token = await authFacade.Authenticate(username, password);

            Assert.Null(token);
        }
    }
}