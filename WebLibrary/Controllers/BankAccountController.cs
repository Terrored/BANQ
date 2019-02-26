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


        //TODO Add logic to restrict user from creating multiple accounts
        public ActionResult Create()
        {
            var bankAccountTypes = _bankAccountTypeService.GetBankAccountTypes();
            return View(new BankAccountViewModel() { BankAccountTypes = bankAccountTypes, BankAccountDto = new BankAccountDto() });
        }

        [HttpPost]
        public ActionResult Create(BankAccountViewModel bankAccountViewModel)
        {
            if (!ModelState.IsValid)
                return View(bankAccountViewModel);

            var bankAccountDto = new BankAccountDto()
            {
                ApplicationUserId = User.Identity.GetUserId().Value,
                BankAccountTypeId = bankAccountViewModel.BankAccountDto.BankAccountTypeId,
            };
            _bankAccountService.CreateBankAccount(bankAccountDto);

            return RedirectToAction("Index", "UserInfo");
        }
    }
}