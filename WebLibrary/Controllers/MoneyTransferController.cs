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
            ViewBag.Id = User.Identity.GetUserId().Value;
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Transfers()
        {
            ViewBag.Id = User.Identity.GetUserId().Value;
            return View();
        }

    }
}