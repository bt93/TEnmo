using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public TransferType TransferType { get; set; } = TransferType.Send;
        public TransferStatus TransferStatus { get; set; }
        [Required]
        public ReturnUser AccountFrom { get; set; }
        [Required]
        public ReturnUser AccountTo { get; set; }
        [Required]
        [Range(0, 10000)]
        public decimal Amount { get; set; }
    }
}
