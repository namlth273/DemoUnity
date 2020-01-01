using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoUnity.ServiceClients.Abstractions.Services
{
    public interface ITestServiceClient
    {
        Task<IEnumerable<IPost>> GetAsync();
    }

    public interface IPost
    {
        string Type { get; set; }
        string Value { get; set; }
    }

    public interface IVestorlyAuthenticationServiceClient
    {
        Task<SignInAsMasterAccountResponse> SignInAsMasterAccount();
        Task<string> ImpersonateAdvisor();
        Task GetAsync();
    }

    public class SignInAsMasterAccountRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class SignInAsMasterAccountResponse
    {
        [JsonProperty("vestorly-auth")]
        public string AccessToken { get; set; }
        [JsonProperty("user")]
        public User UserModel { get; set; }

        public class User
        {
            [JsonProperty("_id")]
            public string Id { get; set; }
        }
    }
}