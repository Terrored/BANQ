using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using System.Web.Http;

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
            if (creditDto == null || creditDto.PercentageRate <= 0 || creditDto.CreditAmount <= 0)
                return BadRequest();
            else
                return Ok(_creditService.GetInstallmentAmount(creditDto));
        }
    }
}
