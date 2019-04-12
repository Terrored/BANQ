using BusinessLogic.Interfaces;
using System.Web.Mvc;
using WebLibrary.IdentityExtensions;

namespace WebLibrary.Controllers
{
    [Authorize]
    public class CreditsController : Controller
    {
        private readonly ICreditService _creditService;

        public CreditsController(ICreditService creditService)
        {
            _creditService = creditService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ObtainCredit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ConfirmCredit()
        {
            var userId = User.Identity.GetUserId().Value;

            _creditService.ConfirmCredit(userId);

            return RedirectToAction("Index", "UserInfo");
        }
    }
}