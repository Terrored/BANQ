using System.Web.Mvc;

namespace WebLibrary.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "UserInfo");
            else
                return View();
        }
    }
}