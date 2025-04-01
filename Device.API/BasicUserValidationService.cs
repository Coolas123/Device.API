using AspNetCore.Authentication.Basic;

namespace Device.API
{
    public class BasicUserValidationService : IBasicUserValidationService
    {
        private readonly IConfiguration configuration;

        public BasicUserValidationService(IConfiguration configuration) {
            this.configuration = configuration;
        }

        public Task<bool> IsValidAsync(string username, string password) {
            var storedUsername = configuration.GetSection("Account:username").Value;
            var storedPassword = configuration.GetSection("Account:password").Value;
            bool isAuthenticated = storedUsername == username && storedPassword == password;
            return Task.FromResult(isAuthenticated);
        }
    }
}
