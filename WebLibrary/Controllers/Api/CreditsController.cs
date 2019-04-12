using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using System.Web.Http;
using WebLibrary.IdentityExtensions;

namespace WebLibrary.Controllers.Api
{
    public class CreditsController : ApiController
    {
        private readonly ICreditService _creditService;

        public CreditsController(ICreditService creditService)
        {
            _creditService = creditService;
        }

        [HttpPost]
        public IHttpActionResult LoanCalculator(CreditDto creditDto)
        {
            var result = _creditService.GetCalculatedInstallment(creditDto);

            if (!result.Success)
                return BadRequest(result.Message);
            else
                return Ok(result.Message);
        }

        [HttpPost]
        public IHttpActionResult ObtainCredit(CreditDto creditDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Provided data is incorrect");

            var userId = User.Identity.GetUserId().Value;
            creditDto.UserId = userId;

            var result = _creditService.CreateCredit(creditDto);

            if (!result.Success)
                return BadRequest(result.Message);
            else
                return Ok(result.Message);
        }
    }
}
