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
        public AccountController(IAccountDAO accountDAO)
        {
            this.accountSqlDAO = accountDAO;
        }
        [HttpGet("{id}")]
        public ActionResult<decimal> GetBalance(int id)
        {
            Account balance =  accountSqlDAO.GetBalance(id);

            return balance.Balance;
            
        }


    }
}
