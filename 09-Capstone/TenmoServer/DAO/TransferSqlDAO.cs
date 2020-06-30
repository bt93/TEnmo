using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;
using static TenmoServer.Models.Transfer;

namespace TenmoServer.DAO
{
    public class TransferSqlDAO : ITransferDAO
    {
        private string connectionString;

        public TransferSqlDAO(string dbConnectionString)
        {
            this.connectionString = dbConnectionString;
        }

        public List<Transfer> GetUserTransfers(int id)
        {
            List<Transfer> transfers = new List<Transfer>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sqlCommand = @"SELECT * FROM transfers WHERE account_from = @id OR account_to = @id";
                SqlCommand cmd = new SqlCommand(sqlCommand, conn);
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Transfer transfer = new Transfer();

                    transfer.TransferId = Convert.ToInt32(rdr["transfer_id"]);
                    transfer.TransferType = (TransferType)Convert.ToInt32(rdr["transfer_type_id"]);
                    transfer.TransferStatus = (TransferStatus)Convert.ToInt32(rdr["transfer_status_id"]);
                    transfer.AccountFrom = Convert.ToInt32(rdr["account_from"]);
                    transfer.AccountTo = Convert.ToInt32(rdr["account_to"]);
                    transfer.Amount = Convert.ToDecimal(rdr["amount"]);

                    transfers.Add(transfer);
                }
                return transfers;
            }
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

        public Transfer CreateTransfer(Transfer transfer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                const string sql = @"INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, ammount)
                                        VALUES (@transfer_type_id, @transfer_status_id, @account_from, @account_to, @ammount)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@transfer_type_id", transfer.TransferType);
                cmd.Parameters.AddWithValue("@transfer_status_id", transfer.TransferStatus);
                cmd.Parameters.AddWithValue("@transfer_from", transfer.AccountFrom);
                cmd.Parameters.AddWithValue("@account_to", transfer.AccountTo);
                cmd.Parameters.AddWithValue("@ammount", transfer.Amount);

                cmd.ExecuteNonQuery();
            }
            return transfer;
        }
    }
}
