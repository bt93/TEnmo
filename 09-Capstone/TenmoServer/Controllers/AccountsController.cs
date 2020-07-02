using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.Models;
using TenmoServer.DAO;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private string userName
        {
            get
            {
                return User?.Identity?.Name;
            }
        }

        private int userId
        {
            get
            {
                foreach (Claim claim in User.Claims)
                {
                    if (claim.Type == "sub")
                    {
                        return Convert.ToInt32(claim.Value);
                    }
                }

                return 0;
            }
        }

        private IAccountDAO accountSqlDAO;
        private ITransferDAO transferDAO;
        public AccountsController(IAccountDAO accountDAO, ITransferDAO transferDAO)
        {
            this.accountSqlDAO = accountDAO;
            this.transferDAO = transferDAO;
        }
        /// <summary>
        /// Allows the user to get  
        /// </summary>
        /// <returns></returns>
        [HttpGet("balance")]
        public ActionResult<Account> GetBalance()
        {
            Account account =  accountSqlDAO.GetBalance(userId);

            return account;
            
        }

        [HttpGet("transfers")]
        public ActionResult<List<Transfer>> GetUserTransfers()
        {
            List<Transfer> transfers = transferDAO.GetUserTransfers(userId);

            if (transfers == null)
            {
                
                return NotFound();
            }

            return transfers;
        }
        
        [HttpGet("transfers/{transferId}")]
        public ActionResult<Transfer> GetTransferId(int transferId)
        {
            Transfer transfer = transferDAO.GetTransferById(transferId, userId);
            if (transfer.AccountFrom == null || transfer.AccountTo == null)
            {
                return NotFound();
            }
            return transfer;
        }
        [HttpGet]
        public ActionResult<List<ReturnUser>> GetUsersForTransfer()
        {
            List<ReturnUser> users = transferDAO.GetUsersForTransfer();

            if (users == null)
            {
                return NotFound();
            }
            return users;
        }

        [HttpPost("transfers")]
        public ActionResult<Transfer> StartTransfer(Transfer transfer)
        {
            Account userAccount = accountSqlDAO.GetBalance(userId);
            Transfer newTransfer;
            
            if (userId == transfer.AccountTo.UserId)
            {
                return Forbid();
            }

            if (userAccount.Balance >= transfer.Amount)
            {
                transfer.TransferStatus = TransferStatus.Approved;

                newTransfer = transferDAO.CreateTransfer(transfer);
                transferDAO.InitiateTransfer(newTransfer);

                return Ok(newTransfer);
            }

            transfer.TransferStatus = TransferStatus.Rejected;
            newTransfer = transferDAO.CreateTransfer(transfer);

            return Conflict(newTransfer);
        }
    }
}
