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
        // GET: MoneyTransfer
        public ActionResult Transfer(decimal amount, int toId)
        {
            var userId = User.Identity.GetUserId();
            _moneyTransferService.Transfer(amount, userId.Value, toId);
            return RedirectToAction("Index", "UserInfo");
        }
    }
}