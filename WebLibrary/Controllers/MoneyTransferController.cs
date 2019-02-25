using BusinessLogic.Interfaces;
using System.Net;
using System.Web.Mvc;
using WebLibrary.IdentityExtensions;
using WebLibrary.Models.ViewModels;

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

        [HttpPost]
        public ActionResult Transfer(MoneyTransferViewModel viewModel)
        {
            viewModel.FromId = User.Identity.GetUserId();
            _moneyTransferService.Transfer(viewModel.CashAmount, viewModel.FromId.Value, viewModel.ToId);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult Create()
        {
            return View();
        }

    }
}