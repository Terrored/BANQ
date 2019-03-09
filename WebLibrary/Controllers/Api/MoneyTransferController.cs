using BusinessLogic.Interfaces;
using DataAccess.Identity;
using Newtonsoft.Json.Linq;
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

        [HttpPost]
        public IHttpActionResult Transfer(JObject data)
        {
            var fromId = User.Identity.GetUserId();

            dynamic transfer = data;
            decimal cash = transfer.cashAmount;
            string name = transfer.name;
            int toId = transfer.toId;

            var dto = _moneyTransferService.Transfer(name, cash, fromId.Value, toId);

            return Ok(new { message = dto.Message });
        }
    }
}
