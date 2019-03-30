using BusinessLogic.Interfaces;
using System.Web.Mvc;

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


    }
}