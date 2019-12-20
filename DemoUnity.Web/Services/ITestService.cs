using System;
using System.Threading.Tasks;

namespace DemoUnity.Web.Services
{
    public interface ITestService
    {
        Task HandleAsync();
    }

    public class TestService : ITestService
    {
        public Task HandleAsync()
        {
            Console.WriteLine("Hello");

            return Task.CompletedTask;
        }
    }
}