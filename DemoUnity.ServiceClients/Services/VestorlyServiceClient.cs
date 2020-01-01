using DemoUnity.ServiceClients.Abstractions.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace DemoUnity.ServiceClients.Services
{
    public class VestorlyAuthenticationServiceClient : IVestorlyAuthenticationServiceClient
    {
        private readonly HttpClient _httpClient;

        public VestorlyAuthenticationServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task GetAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<SignInAsMasterAccountResponse> SignInAsMasterAccount()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/session_management/sign_in")
            {
                Content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("username", "ali.baldari@fmgsuite.com"),
                    new KeyValuePair<string, string>("password", "bc6d9d7ae4295a319c55")
                })
            };

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var streamReader = new StreamReader(stream))
            {
                var content = streamReader.ReadToEnd();

                var accessToken = JsonConvert.DeserializeObject<SignInAsMasterAccountResponse>(content);

                return accessToken;
            }
        }

        public async Task<string> ImpersonateAdvisor()
        {
            var adminId = "5c34c7b253763b00044fba81";
            var advisorId = "5dfae1f62491fb0013e15098";

            var request = new HttpRequestMessage(HttpMethod.Post, $"api/admins/{adminId}/advisors/{advisorId}/impersonate_advisor");

            request.Headers.Add("x-vestorly-auth", "JWT%20eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9%2EeyJzdWIiOiI1YzU5ZmU2Zjg3ZTJjYjAwMDQ0YjU1YTMiLCJpYXQiOjE1Nzc4NDYyNjUsImV4cCI6MTU3OTA1NTg2NX0%2ED_Dq4OmZSLIecg8l9ZkFbm4fc6gRnKBIpTtkxabxUTI");

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var streamReader = new StreamReader(stream))
            {
                var content = streamReader.ReadToEnd();

                var accessToken = JsonConvert.DeserializeObject<ImpersonateAdvisorResponse>(content);

                return accessToken.AccessToken;
            }
        }

        public class ImpersonateAdvisorResponse
        {
            [JsonProperty("vestorly_auth")]
            public string AccessToken { get; set; }
        }
    }
}