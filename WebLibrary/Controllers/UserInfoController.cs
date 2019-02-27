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

            var userInfo = new UserInfoViewModel()
            {
                BankAccount = _bankAccountService.GetBankAccountDetails(currentUserId),
                UserName = User.Identity.Name
            };
            return View(userInfo);
        }
    }
}