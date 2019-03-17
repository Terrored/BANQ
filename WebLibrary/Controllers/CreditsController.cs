using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using System.Web.Mvc;
using WebLibrary.IdentityExtensions;
using WebLibrary.Models;

namespace WebLibrary.Controllers
{
    public class CreditsController : Controller
    {
        private readonly ICreditService _creditService;

        public CreditsController(ICreditService creditService)
        {
            _creditService = creditService;
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
                InstallmentCount = model.InstallmentCount,
                CreditAmount = model.CreditAmount,
                PercentageRate = model.PercentageRate,
                Confirmed = false,
                UserId = userId
            };

            _creditService.CreateCredit(creditDto);
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