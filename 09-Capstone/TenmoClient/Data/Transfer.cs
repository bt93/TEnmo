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
       
        public int TransferId { get; set; }
        public TransferType TransferType { get; set; }
        public TransferStatus TransferStatus { get; set; }
        public ReturnUser AccountFrom { get; set; }
        public ReturnUser AccountTo { get; set; }
        public decimal Amount { get; set; }

        public override string ToString()
        {
            //23          From: Bernice          $ 903.14
            if (TransferType == TransferType.Request)
            {
                return $"{TransferId,8} From: {AccountFrom.Username,10} Balance: {Amount:C}";
            }
            else
            {
                return $"{TransferId,8} To: {AccountTo.Username,12} Balance: {Amount:C}";
            }
        }

        public string DetailsForTransfer()
        {
            return $"ID: {TransferId} \n From: {AccountFrom.Username} \n To: {AccountTo.Username} \n Type: {this.TransferType} \n Status: {this.TransferStatus} \n Amount: {Amount:C}";
        }
    }
}
