﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronPassword
{
    public class Account
    {
        public string AccountName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public DateTime CreationTime { get; set; }
        public int ID { get; set; }

        public Account()
        {
            this.CreationTime = DateTime.Now;
            this.ID = AccountManager.safe.getNextID();
        }
    }
}
