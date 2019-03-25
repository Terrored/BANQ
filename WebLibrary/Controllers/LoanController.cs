using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using System.Web.Mvc;
using WebLibrary.IdentityExtensions;

namespace WebLibrary.Controllers
{
    public class LoanController : Controller
    {
        private readonly ILoanService _loanService;

        public LoanController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        // GET: Loan
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TakeLoan()
        {
            var userId = HttpContext.User.Identity.GetUserId().Value;
            var loan = new LoanDto()
            {
                LoanAmount = 100M,
                PercentageRate = 5,
                TotalInstallments = 12,
                UserId = userId
            };

            _loanService.TakeLoan(loan);

            return View("Index");
        }

        public ActionResult PayInstallment()
        {
            var userId = HttpContext.User.Identity.GetUserId().Value;
            //Hardcoded for developer testing
            var installment = new LoanInstallmentDto()
            {
                LoanId = 3
            };
            _loanService.PayInstallment(installment);

            return View("Index");
        }
    }
}