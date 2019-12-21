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
        int Id { get; set; }
        string Title { get; set; }
    }
}