using BusinessLogic.Interfaces;
using System.Web.Mvc;
using WebLibrary.IdentityExtensions;
using WebLibrary.Models;

namespace WebLibrary.Controllers
{
    [Authorize]
    public class UserInfoController : Controller
    {
        private readonly IBankAccountService _bankAccountService;

        public UserInfoController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }

        public ActionResult Index()
        {
            var currentUserId = User.Identity.GetUserId().Value;
            var bankAccount = _bankAccountService.GetBankAccountDetails(currentUserId);
            var sex = User.Identity.GetUserSex();

            if (bankAccount == null)
            {
                return RedirectToAction("Create", "BankAccount");
            }
            else
            {
                var userInfo = new UserInfoViewModel()
                {
                    BankAccount = bankAccount,
                    UserName = User.Identity.Name,
                    UserSex = sex
                };
                return View(userInfo);
            }
        }
    }
}