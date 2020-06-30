using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Account
    {
        public Account()
        {
            
        }

        public Account(int accountId, int userId, decimal balance)
        {
            this.AccountId = accountId;
            this.UserId = userId;
            this.Balance = balance;
        }

        public int AccountId { get; set; }
        public int UserId { get; set; }
        public decimal Balance { get; set; }
    }
}
