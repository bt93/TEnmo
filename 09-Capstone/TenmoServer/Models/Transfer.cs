using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }
        public enum TransferType
        {
            Request = 1,
            Send  = 2
        }
        public enum TransferStatus
        {
            Pending = 1,
            Approved = 2,
            Rejected = 3
        }
        public int AccountFrom { get; set; }
        public int AccountTo { get; set; }
        public decimal Amount { get; set; }
    }
}
