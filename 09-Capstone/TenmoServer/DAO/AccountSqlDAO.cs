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

        public Account GetBalance(int id)
        {
            Account account = new Account();

            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();

                
                const string sql = @"SELECT * FROM accounts JOIN users ON accounts.user_id = users.user_id WHERE users.user_id = @user_id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@user_id", id);

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    account.Balance = Convert.ToDecimal(rdr["balance"]);
                    account.UserName = Convert.ToString(rdr["username"]);
                    account.AccountId = Convert.ToInt32(rdr["account_id"]);
                    account.UserId = Convert.ToInt32(rdr["user_id"]);
                }
                

                return account;
            }
        }
    }
}
