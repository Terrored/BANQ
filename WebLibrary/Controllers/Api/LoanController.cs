using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using System.Web.Http;
using WebLibrary.IdentityExtensions;

namespace WebLibrary.Controllers.Api
{
    [Authorize]
    public class LoanController : ApiController
    {
        private readonly ILoanService _loanService;

        public LoanController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpPost]
        public IHttpActionResult TakeLoan(LoanDto loanDto)
        {

            loanDto.UserId = User.Identity.GetUserId().Value;

            _loanService.TakeLoan(loanDto);
            return Ok();
        }
        [HttpPost]
        public IHttpActionResult PayInstallment(LoanInstallmentDto installmentDto)
        {
            var result = _loanService.PayInstallment(installmentDto);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetLoans()
        {
            var userId = User.Identity.GetUserId().Value;
            var loans = _loanService.GetLoans(userId);
            return Ok(loans);
        }
    }
}
