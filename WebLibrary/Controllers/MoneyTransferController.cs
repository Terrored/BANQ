using BusinessLogic.Interfaces;
using Newtonsoft.Json;
using System.Web.Mvc;
using WebLibrary.IdentityExtensions;

namespace WebLibrary.Controllers
{
    public class MoneyTransferController : Controller
    {
        private readonly IMoneyTransferService _moneyTransferService;
        private readonly IBankAccountService _bankAccountService;
        public MoneyTransferController(IMoneyTransferService moneyTransferService, IBankAccountService bankAccountService)
        {
            _moneyTransferService = moneyTransferService;
            _bankAccountService = bankAccountService;
        }

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public string GetLastTransfers()
        {
            var userId = HttpContext.User.Identity.GetUserId();
            var lastTransfers = _moneyTransferService.GetLastSentFiveTransfers(userId.Value);
            var x = JsonConvert.SerializeObject(lastTransfers, Formatting.Indented);
            return x;

        }

        [HttpPost]
        public ActionResult Transfer(string name, decimal cashAmount, int toId)
        {
            var fromId = User.Identity.GetUserId();

            var dto = _moneyTransferService.Transfer(name, cashAmount, fromId.Value, toId);

            return Json(new { message = dto.Message });
        }

        public ActionResult Create()
        {
            return View();
        }

    }
}