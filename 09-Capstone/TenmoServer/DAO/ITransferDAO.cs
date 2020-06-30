using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDAO
    {
        Transfer CreateTransfer(Transfer transfer);
        List<ReturnUser> GetUsersForTransfer();
        List<Transfer> GetUserTransfers(int id);
        void InitiateTransfer(Account fromUser, Account toUser, decimal transferAmount);
    }
}