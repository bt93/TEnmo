using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public enum TransferType
    {
        Request = 1,
        Send = 2
    }
    public enum TransferStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3
    }
    public class Transfer
    {
        //TODO: Change user back to int 
        public int TransferId { get; set; }
        public TransferType TransferType { get; set; }
        public TransferStatus TransferStatus { get; set; }
        public ReturnUser AccountFrom { get; set; }
        public ReturnUser AccountTo { get; set; }
        public decimal Amount { get; set; }
    }
}
