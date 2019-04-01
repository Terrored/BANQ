using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using System.Web.Mvc;
using WebLibrary.IdentityExtensions;
using WebLibrary.Models;

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
            return View(new CreditViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ObtainCredit(CreditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = User.Identity.GetUserId().Value;

            var creditDto = new CreditDto()
            {
                InstallmentCount = model.CreditPeriodInYears * 12,
                CreditAmount = model.CreditAmount,
                UserId = userId
            };

            var result = _creditService.CreateCredit(creditDto);
            ViewBag.Message = result.Message;

            if (!result.Success)
                return View(model);
            else
                return View("ConfirmationPrompt");
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