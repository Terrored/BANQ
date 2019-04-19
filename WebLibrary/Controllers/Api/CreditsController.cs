using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using System.Web.Http;
using WebLibrary.IdentityExtensions;

namespace WebLibrary.Controllers.Api
{
    [Authorize]
    public class CreditsController : ApiController
    {
        private readonly ICreditService _creditService;

        public CreditsController(ICreditService creditService)
        {
            _creditService = creditService;
        }

        [HttpGet]
        public IHttpActionResult Calculate([FromUri]CreditDto creditDto)
        {
            var result = _creditService.GetCalculatedInstallment(creditDto);

            if (!result.Success)
                return BadRequest(result.Message);
            else
                return Ok(result.Message);
        }

        [HttpGet]
        public IHttpActionResult GetPercentageRate([FromUri]CreditDto creditDto)
        {
            var userId = User.Identity.GetUserId().Value;
            creditDto.UserId = userId;

            var result = _creditService.GetCalculatedPercentageRate(creditDto);

            if (!result.Success)
            {
                return BadRequest();
            }
            else
            {
                return Ok(result.Message);
            }
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

        [HttpGet]
        public IHttpActionResult Installments(int id)
        {
            var userId = User.Identity.GetUserId().Value;
            var installments = _creditService.GetInstallmentsForCredit(userId, id);

            if (installments == null)
                return BadRequest("The credit does not exist or you don't have permission to access it");
            else
                return Ok(installments);
        }

        [HttpPost]
        public IHttpActionResult PayInstallment(CreditInstallmentDto creditInstallmentDto)
        {
            var userId = User.Identity.GetUserId().Value;

            var result = _creditService.PayInstallment(userId, creditInstallmentDto);

            if (!result.Success)
                return BadRequest(result.Message);
            else
                return Ok(result.Message);
        }
    }
}
