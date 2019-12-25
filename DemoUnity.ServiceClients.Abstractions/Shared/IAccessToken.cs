namespace DemoUnity.ServiceClients.Abstractions.Shared
{
    public interface IAccessToken
    {
        string Token { get; set; }
    }

    public class AccessToken : IAccessToken
    {
        public string Token { get; set; }
    }
}