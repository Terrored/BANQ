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

            var result = _loanService.TakeLoan(loanDto);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }

        }
        [HttpPost]
        public IHttpActionResult PayInstallment(LoanInstallmentDto installmentDto)
        {
            var result = _loanService.PayInstallment(installmentDto);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }

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
