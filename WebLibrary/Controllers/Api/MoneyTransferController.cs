using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using DataAccess.Identity;
using System.Web.Http;
using WebLibrary.IdentityExtensions;

namespace WebLibrary.Controllers.Api
{
    [Authorize]
    public class MoneyTransferController : ApiController
    {
        private readonly IMoneyTransferService _moneyTransferService;
        private readonly IApplicationUserManager _userManager;

        public MoneyTransferController(IMoneyTransferService moneyTransferService, IApplicationUserManager userManager)
        {
            _moneyTransferService = moneyTransferService;
            _userManager = userManager;
        }

        [HttpGet]
        public IHttpActionResult GetLastFive()
        {
            var userId = User.Identity.GetUserId().Value;
            var transfers = _moneyTransferService.GetLastSentFiveTransfers(userId);
            return Ok(transfers);
        }

        [HttpGet]
        public IHttpActionResult GetAllTransfers()
        {
            var userId = User.Identity.GetUserId().Value;
            var transfers = _moneyTransferService.GetAllTransfers(userId);
            return Ok(transfers);
        }

        [HttpPost]
        public IHttpActionResult Transfer(MoneyTransferDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            dto.From.Id = User.Identity.GetUserId().Value;

            var result = _moneyTransferService.Transfer(dto);


            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            else
            {
                return Ok(result.Message);
            }
        }
    }
}
