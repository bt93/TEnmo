using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.Models;
using TenmoServer.DAO;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountDAO accountSqlDAO;
        private ITransferDAO transferDAO;
        public AccountController(IAccountDAO accountDAO, ITransferDAO transferDAO)
        {
            this.accountSqlDAO = accountDAO;
            this.transferDAO = transferDAO;
        }
        [HttpGet("{id}")]
        public ActionResult<decimal> GetBalance(int id)
        {
            Account balance =  accountSqlDAO.GetBalance(id);

            return balance.Balance;
            
        }

        [HttpGet("transfers/{id}")]
        public ActionResult<List<Transfer>> GetUserTransfers(int id)
        {
            List<Transfer> transfers = transferDAO.GetUserTransfers(id);

            if (transfers == null)
            {
                return NotFound();
            }

            return transfers;
        }
    }
}
