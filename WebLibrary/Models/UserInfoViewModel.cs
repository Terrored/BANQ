using BusinessLogic.DTOs;

namespace WebLibrary.Models
{
    public class UserInfoViewModel
    {
        public BankAccountDto BankAccount { get; set; }
        public string UserFirstName { get; set; }
        public string UserSex { get; set; }
        public string UserLastName { get; set; }
        public bool UnconfirmedCredit { get; set; }
        public LoanDto LastLoan { get; set; }
        public int? LoansTaken { get; set; }

        public string IconClass
        {
            get
            {
                if (UserSex == "Female")
                    return "icon-woman";
                else
                    return "icon-man";
            }
        }
    }
}