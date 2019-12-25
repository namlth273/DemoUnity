using DemoUnity.ServiceClients.Abstractions.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DemoUnity.WebAutofac.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITestServiceClient _testServiceClient;

        public HomeController(ITestServiceClient testServiceClient)
        {
            _testServiceClient = testServiceClient;
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

            return Json(posts.Count(), JsonRequestBehavior.AllowGet);
        }
    }
}