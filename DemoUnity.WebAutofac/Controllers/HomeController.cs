using DemoUnity.ServiceClients.Abstractions.Services;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DemoUnity.WebAutofac.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITestServiceClient _testServiceClient;
        private readonly IMemoryCache _memoryCache;

        public HomeController(ITestServiceClient testServiceClient, IMemoryCache memoryCache)
        {
            _testServiceClient = testServiceClient;
            _memoryCache = memoryCache;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public async Task<JsonResult> Demo()
        {
            var posts = await _testServiceClient.GetAsync();

            _memoryCache.TryGetValue("DemoUnity.AccessToken", out string accessToken);

            return Json(new { Count = posts.Count(), AccessToken = accessToken }, JsonRequestBehavior.AllowGet);
        }
    }
}