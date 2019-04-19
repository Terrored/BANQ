using BusinessLogic.Interfaces;
using System.Linq;
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
            var userId = User.Identity.GetUserId().Value;
            var sex = User.Identity.GetUserSex();
            var lastName = User.Identity.GetUserLastName();
            var loans = _loanService.GetLoans(userId);
            var lastLoan = loans.Where(l => l.InstallmentsLeft > 0).OrderBy(l => l.DateTaken).FirstOrDefault();

            var bankAccount = _bankAccountService.GetBankAccountDetails(userId);

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
                    UnconfirmedCredit = _bankAccountService.HasUnconfirmedCredit(userId),
                    LastLoan = lastLoan,
                    LoansTaken = loans.Count
                };
                return View(userInfo);
            }
        }
    }
}