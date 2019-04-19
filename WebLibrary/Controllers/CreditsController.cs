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
            var userId = User.Identity.GetUserId().Value;
            var currentCredit = _creditService.GetCurrentCreditInfo(userId);

            return View(currentCredit);
        }

        public ActionResult ObtainCredit()
        {
            return View();
        }

        public ActionResult InstallmentsPage()
        {
            var userId = User.Identity.GetUserId().Value;
            var currentCredit = _creditService.GetCurrentCreditInfo(userId);

            if (currentCredit == null)
                return RedirectToAction("Index", "UserInfo");
            else
                return View(currentCredit);
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