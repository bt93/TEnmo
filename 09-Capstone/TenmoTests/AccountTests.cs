using Microsoft.VisualStudio.TestTools.UnitTesting;
using TenmoServer.DAO;
using TenmoServer.Models;
using TenmoServer.Security;
using TenmoServer.Controllers;
using RestSharp;

namespace TenmoTests
{
    [TestClass]
    public class AccountTests
    {

        // TODO: Come back to create tests
        [TestMethod]
        public void TestGetBalance()
        {
            //// Arrange
            //AccountSqlDAO dao = new AccountSqlDAO("connectionstring");
            //Account account = new Account(1, 1, 1000);

            //// Act
            //decimal expect = dao.GetBalance(account.UserId);

            //// Assert
            //Assert.AreEqual(1000, expect);
        }

        [TestMethod]
        public void NewUserAccountBalanceIs1000()
        {
            string API_URL_LOGIN = "https://localhost:44315/login";
            RestClient client = new RestClient(API_URL_LOGIN);


        }
    }
}
