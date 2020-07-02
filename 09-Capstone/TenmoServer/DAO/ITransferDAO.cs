using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDAO
    {
        Transfer CreateTransfer(Transfer transfer);
        Transfer GetTransferById(int id, int userId);
        ReturnUser GetUserById(int id);
        List<ReturnUser> GetUsersForTransfer();
        List<Transfer> GetUserTransfers(int id);
        void InitiateTransfer(Transfer transfer);
    }
}