using System.Web.Mvc;
using WebLibrary.IdentityExtensions;

namespace WebLibrary.Controllers
{
    [Authorize]
    public class UserInfoController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.x = User.Identity.GetUserId();
            return View();
        }
    }
}