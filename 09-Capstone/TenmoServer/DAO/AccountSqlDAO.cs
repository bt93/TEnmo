using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class AccountSqlDAO : IAccountDAO
    {
        private readonly string connectionString;

        public AccountSqlDAO(string dbconnectionString)
        {
            this.connectionString = dbconnectionString;
        }

        public decimal GetBalance(int id)
        {
            Account account = new Account();

            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();

                const string sql = @"SELECT balance FROM accounts WHERE user_id = @user_id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@user_id", id);

                SqlDataReader rdr = cmd.ExecuteReader();
                account.Balance = Convert.ToDecimal(rdr["balance"]);

                return account.Balance;
            }
        }
    }
}
