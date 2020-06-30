using Microsoft.VisualStudio.TestTools.UnitTesting;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoTests
{
    [TestClass]
    public class AccountTests
    {

        // TODO: Come back to create tests
        [TestMethod]
        public void TestGetBalance()
        {
            // Arrange
            AccountSqlDAO dao = new AccountSqlDAO("connectionstring");
            Account account = new Account(1, 1, 1000);

            // Act
            decimal expect = dao.GetBalance(account.UserId);

            // Assert
            Assert.AreEqual(1000, expect);
        }
    }
}
