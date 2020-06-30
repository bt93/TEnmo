using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDAO
    {
        private string connectionString;

        public TransferSqlDAO(string dbConnectionString)
        {
            this.connectionString = dbConnectionString;
        }

        public List<User> GetUsersForTransfer()
        {
            List<User> allUsers = new List<User>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sqlCommand = @"SELECT * FROM users";
                SqlCommand cmd = new SqlCommand(sqlCommand, conn);

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {

                    User user = new User();
                    user.UserId = Convert.ToInt32(rdr["user_id"]);
                    user.Username = Convert.ToString(rdr["username"]);
                    allUsers.Add(user);
                }
                
            }
            return allUsers;
        }

        public void InitiateTransfer(Account fromUser, Account toUser, decimal transferAmount)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlUpdateFrom = @"UPDATE accounts SET Balance = @fromUserBalance where user_id = @fromUser";
                string sqlUpdateTo = @"UPDATE accounts SET balance = @toUserBalance where user_id = @toUser";

                conn.Open();
                //Update the from user 
                SqlCommand cmd = new SqlCommand(sqlUpdateFrom, conn);
                cmd.Parameters.AddWithValue("@fromUser", fromUser.UserId);
                cmd.Parameters.AddWithValue("@fromUserBalance", fromUser.Balance);
                cmd.ExecuteNonQuery();
                //Update the To User 
                cmd = new SqlCommand(sqlUpdateTo, conn);
                cmd.Parameters.AddWithValue("@toUser", toUser.UserId);
                cmd.Parameters.AddWithValue("@toUserBalance", toUser.Balance);
                cmd.ExecuteNonQuery();
                
            }
        }

    }
}
