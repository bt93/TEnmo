using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Account
    {
        // TODO: add data validation
        public string UserName { get; set; }
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public decimal Balance { get; set; }
    }
}
