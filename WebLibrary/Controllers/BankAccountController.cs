using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using System.Web.Mvc;
using WebLibrary.IdentityExtensions;
using WebLibrary.Models;

namespace WebLibrary.Controllers
{
    [Authorize]
    public class BankAccountController : Controller
    {
        private readonly IBankAccountService _bankAccountService;
        private readonly IBankAccountTypeService _bankAccountTypeService;

        public BankAccountController(IBankAccountService bankAccountService, IBankAccountTypeService bankAccountTypeService)
        {
            _bankAccountService = bankAccountService;
            _bankAccountTypeService = bankAccountTypeService;
        }

        public ActionResult Create()
        {
            if (_bankAccountService.UserAlreadyHasAccount(User.Identity.GetUserId().Value))
                return RedirectToAction("Index", "UserInfo");
            else
            {
                var bankAccountTypes = _bankAccountTypeService.GetBankAccountTypes();
                return View(new BankAccountViewModel() { BankAccountTypes = bankAccountTypes, BankAccountDto = new BankAccountDto() });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BankAccountViewModel bankAccountViewModel)
        {
            var currentUserId = User.Identity.GetUserId().Value;

            if (_bankAccountService.UserAlreadyHasAccount(currentUserId))
                return RedirectToAction("Index", "UserInfo");

            if (!ModelState.IsValid)
                return View(bankAccountViewModel);

            var bankAccountDto = new BankAccountDto()
            {
                ApplicationUserId = currentUserId,
                BankAccountTypeId = bankAccountViewModel.BankAccountDto.BankAccountTypeId,
            };
            _bankAccountService.CreateBankAccount(bankAccountDto);

            return RedirectToAction("Index", "UserInfo");
        }
    }
}