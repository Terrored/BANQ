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
        private readonly ILoanService _loanService;

        public UserInfoController(IBankAccountService bankAccountService, ILoanService loanService)
        {
            _bankAccountService = bankAccountService;
            _loanService = loanService;
        }

        public ActionResult Index()
        {
            var currentUserId = User.Identity.GetUserId().Value;
            var sex = User.Identity.GetUserSex();
            var lastName = User.Identity.GetUserLastName();

            var bankAccount = _bankAccountService.GetBankAccountDetails(currentUserId);

            if (bankAccount == null)
            {
                return RedirectToAction("Create", "BankAccount");
            }
            else
            {
                var userInfo = new UserInfoViewModel()
                {
                    BankAccount = bankAccount,
                    UserFirstName = User.Identity.Name,
                    UserLastName = lastName,
                    UserSex = sex,
                    UnconfirmedCredit = _bankAccountService.HasUnconfirmedCredit(currentUserId)
                };
                return View(userInfo);
            }
        }

        public ActionResult Loan()
        {
            var userId = HttpContext.User.Identity.GetUserId();
            //_loanService.TakeLoan(userId.Value);

            return View("Index");

        }
    }
}