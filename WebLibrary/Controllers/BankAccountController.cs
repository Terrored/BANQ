using BusinessLogic;
using System.Web.Mvc;
using WebLibrary.IdentityExtensions;

namespace WebLibrary.Controllers
{
    [Authorize]
    public class BankAccountController : Controller
    {
        private readonly IBankAccountService _bankAccountService;
        public BankAccountController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }

        [HttpPost]
        public ActionResult Create()
        {
            var id = HttpContext.User.Identity.GetUserId().Value;
            var bankAccountDto = _bankAccountService.CreateBankAccount(id);
            return View(bankAccountDto);
        }
    }
}