using BusinessLogic.Interfaces;
using System.Web.Mvc;

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

        public ActionResult Create()
        {
            return View();
        }

    }
}