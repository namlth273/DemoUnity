using DemoUnity.ServiceClients.Abstractions.Services;
using DemoUnity.Web.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DemoUnity.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITestService _testService;
        private readonly ITestServiceClient _testServiceClient;

        public HomeController(ITestService testService, ITestServiceClient testServiceClient)
        {
            _testService = testService;
            _testServiceClient = testServiceClient;
        }

        public async Task<ActionResult> Index()
        {
            await _testService.HandleAsync();

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

            return Json(posts.Count(), JsonRequestBehavior.AllowGet);
        }
    }
}