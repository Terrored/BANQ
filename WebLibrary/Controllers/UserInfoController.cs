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
        private readonly ICreditService _creditService;

        public UserInfoController(IBankAccountService bankAccountService, ILoanService loanService, ICreditService creditService)
        {
            _bankAccountService = bankAccountService;
            _loanService = loanService;
            _creditService = creditService;
        }

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId().Value;
            var bankAccount = _bankAccountService.GetBankAccountDetails(userId);

            if (bankAccount == null)
            {
                return RedirectToAction("Create", "BankAccount");
            }

            var sex = User.Identity.GetUserSex();
            var lastName = User.Identity.GetUserLastName();
            var loans = _loanService.GetLoans(userId);
            var lastLoan = loans.Where(l => l.InstallmentsLeft > 0).OrderBy(l => l.DateTaken).FirstOrDefault();
            var activeCredit = _creditService.HasActiveCredit(userId);

            var userInfo = new UserInfoViewModel()
            {
                BankAccount = bankAccount,
                UserFirstName = User.Identity.Name,
                UserLastName = lastName,
                UserSex = sex,
                UnconfirmedCredit = _bankAccountService.HasUnconfirmedCredit(userId),
                LastLoan = lastLoan,
                LoansTaken = loans.Count,
                ActiveCredit = activeCredit.Success ? activeCredit.Data : null
            };
            return View(userInfo);
        }
    }
}