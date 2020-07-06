using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoClient.Data
{
    public class Account
    {
        public string UserName { get; set; }
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public decimal Balance { get; set; }
    }
}
