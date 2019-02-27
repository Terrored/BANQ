﻿using BusinessLogic.DTOs;

namespace WebLibrary.Models
{
    public class UserInfoViewModel
    {
        public BankAccountDto BankAccount { get; set; }
        public string UserName { get; set; }
        public string UserSex { get; set; }

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