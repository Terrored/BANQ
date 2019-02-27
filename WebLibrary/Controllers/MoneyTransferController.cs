using BusinessLogic.Interfaces;
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
            var userId = HttpContext.User.Identity.GetUserId();
            var transfers = _moneyTransferService.GetLastSentFiveTransfers(userId.Value);

            return View(transfers);
        }

        [HttpPost]
        public ActionResult Transfer(decimal cashAmount, int toId)
        {
            var fromId = User.Identity.GetUserId();

            var dto = _moneyTransferService.Transfer(cashAmount, fromId.Value, toId);

            return Json(new { message = dto.Message });
        }

        public ActionResult Create()
        {
            return View();
        }

    }
}