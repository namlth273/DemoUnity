using DemoUnity.ServiceClients.Abstractions.Services;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DemoUnity.WebAutofac.Controllers
{
    public class DemoController : Controller
    {
        private readonly IVestorlyAuthenticationServiceClient _vestorlyAuthenticationServiceClient;

        public DemoController(IVestorlyAuthenticationServiceClient vestorlyAuthenticationServiceClient)
        {
            _vestorlyAuthenticationServiceClient = vestorlyAuthenticationServiceClient;
        }

        public async Task<JsonResult> Demo()
        {
            return Json(await _vestorlyAuthenticationServiceClient.ImpersonateAdvisor(), JsonRequestBehavior.AllowGet);
        }
    }
}