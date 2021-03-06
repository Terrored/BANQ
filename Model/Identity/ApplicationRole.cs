﻿using System.Collections.Generic;

namespace Model.Identity
{
    public class ApplicationRole
    {
        public ApplicationRole()
        {
            Users = new List<ApplicationUserRole>();
        }

        public int Id
        {
            get; set;
        }

        public ICollection<ApplicationUserRole> Users { get; private set; }

        public string Name { get; set; }
    }
}
