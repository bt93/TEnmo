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
    }
}
