using DemoUnity.ServiceClients.Abstractions.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace DemoUnity.ServiceClients.Services
{
    public class TestServiceClient : ITestServiceClient
    {
        private readonly HttpClient _httpClient;

        public TestServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<IPost>> GetAsync()
        {
            Console.WriteLine("Hello");

            var response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "/posts"));

            response.EnsureSuccessStatusCode();

            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var streamReader = new StreamReader(stream))
            {
                var content = streamReader.ReadToEnd();

                var posts = JsonConvert.DeserializeObject<IEnumerable<Post>>(content);

                return posts;
            }
        }

        public class Post : IPost
        {
            public int Id { get; set; }
            public string Title { get; set; }
        }
    }
}