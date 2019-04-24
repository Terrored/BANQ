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
            var credits = _creditService.GetCredits(userId);

            return View(credits);
        }

        public ActionResult ObtainCredit()
        {
            return View();
        }

        public ActionResult InstallmentsPage(int Id)
        {
            var userId = User.Identity.GetUserId().Value;
            var creditInfo = _creditService.GetCreditInfo(userId, Id);

            if (!creditInfo.Success)
                return RedirectToAction("Index", "UserInfo");
            else
                return View(creditInfo.Data);
        }

        [HttpPost]
        public ActionResult ConfirmCredit()
        {
            var userId = User.Identity.GetUserId().Value;

            _creditService.ConfirmCredit(userId);

            return RedirectToAction("Index", "UserInfo");
        }

        public ActionResult CreditHistory()
        {
            var userId = User.Identity.GetUserId().Value;

            var credits = _creditService.GetCredits(userId);

            return View(credits);
        }
    }
}