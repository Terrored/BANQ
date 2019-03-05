using BusinessLogic.Interfaces;
using System.Web.Http;
using WebLibrary.IdentityExtensions;

namespace WebLibrary.Controllers.Api
{
    public class MoneyTransferController : ApiController
    {
        private readonly IMoneyTransferService _moneyTransferService;

        public MoneyTransferController(IMoneyTransferService moneyTransferService)
        {
            _moneyTransferService = moneyTransferService;
        }

        [HttpGet]
        public IHttpActionResult GetLastFive()
        {
            var userId = User.Identity.GetUserId().Value;
            var transfers = _moneyTransferService.GetLastSentFiveTransfers(userId);
            return Ok(transfers);
        }
    }
}
