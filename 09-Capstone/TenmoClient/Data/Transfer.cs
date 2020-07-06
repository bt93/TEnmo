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
       public Transfer(int toUserId, int fromUserId, decimal amount)
        {
            ReturnUser toUser = new ReturnUser();
            toUser.UserId = toUserId;
            ReturnUser fromUser = new ReturnUser();
            fromUser.UserId = fromUserId;

            this.AccountFrom = fromUser;
            this.AccountTo = toUser;
            this.Amount = amount;
        }

        public Transfer()
        {

        }

        public int TransferId { get; set; }
        public TransferType TransferType { get; set; } = TransferType.Send;
        public TransferStatus TransferStatus { get; set; } = TransferStatus.Pending;
        public ReturnUser AccountFrom { get; set; }
        public ReturnUser AccountTo { get; set; }
        public decimal Amount { get; set; }

        public override string ToString()
        {
            //23          From: Bernice          $ 903.14
            if (TransferType == TransferType.Request)
            {
                return $"{TransferId,8} From: {AccountFrom.Username,10} Amount: {Amount:C}";
            }
            else
            {
                return $"{TransferId,8} To: {AccountTo.Username,12} Amount: {Amount:C}";
            }
        }

        public string DetailsForTransfer()
        {
            return $"ID: {TransferId} \n From: {AccountFrom.Username} \n To: {AccountTo.Username} \n Type: {this.TransferType} \n Status: {this.TransferStatus} \n Amount: {Amount:C}";
        }

        public string ReqeustTransfer()
        {
            return $"ID: {TransferId} From: {AccountTo.Username} Status: {this.TransferStatus} Amount: {Amount:C}";
        }
    }
}
