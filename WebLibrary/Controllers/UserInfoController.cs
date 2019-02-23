using System.Web.Mvc;

namespace WebLibrary.Controllers
{
    [Authorize]
    public class UserInfoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}