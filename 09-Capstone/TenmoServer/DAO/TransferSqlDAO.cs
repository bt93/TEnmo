using Microsoft.AspNetCore.Mvc.Formatters.Xml;
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
        public Transfer GetTransferById(int transferId, int userId)
        {
            Transfer transfer = new Transfer();
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // This query only gets transfers that the current user is a part of
                string sqlQuery = @"SELECT * FROM transfers WHERE transfer_id = @transferId AND (account_from = @id OR account_to = @id)";
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@transferId", transferId);
                cmd.Parameters.AddWithValue("@id", userId);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    transfer.TransferId = Convert.ToInt32(rdr["transfer_id"]);
                    transfer.TransferType = (TransferType)Convert.ToInt32(rdr["transfer_type_id"]);
                    transfer.TransferStatus = (TransferStatus)Convert.ToInt32(rdr["transfer_status_id"]);
                    transfer.AccountFrom = GetUserById(Convert.ToInt32(rdr["account_from"]));
                    transfer.AccountTo = GetUserById(Convert.ToInt32(rdr["account_to"]));
                    transfer.Amount = Convert.ToDecimal(rdr["amount"]);
                }
            }
            return transfer;
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
                    //transfer.AccountFrom = )Convert.ToInt32(rdr["account_from"]);
                    //transfer.AccountTo = Convert.ToInt32(rdr["account_to"]);
                    transfer.AccountFrom = GetUserById(Convert.ToInt32(rdr["account_from"]));
                    transfer.AccountTo = GetUserById(Convert.ToInt32(rdr["account_to"]));
                    transfer.Amount = Convert.ToDecimal(rdr["amount"]);

                    transfers.Add(transfer);
                }
                return transfers;
            }
        }

        public List<ReturnUser> GetUsersForTransfer()
        {
            List<ReturnUser> allUsers = new List<ReturnUser>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sqlCommand = @"SELECT * FROM users";
                SqlCommand cmd = new SqlCommand(sqlCommand, conn);

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {

                    ReturnUser user = new ReturnUser();
                    user.UserId = Convert.ToInt32(rdr["user_id"]);
                    user.Username = Convert.ToString(rdr["username"]);
                    allUsers.Add(user);
                }

            }
            return allUsers;
        }

        public void InitiateTransfer(Transfer transfer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlUpdateFrom = @"UPDATE accounts SET Balance = balance - @fromUserBalance where user_id = @fromUser";
                string sqlUpdateTo = @"UPDATE accounts SET balance = balance + @toUserBalance where user_id = @toUser";

                conn.Open();
                //Update the from user 
                SqlCommand cmd = new SqlCommand(sqlUpdateFrom, conn);
                cmd.Parameters.AddWithValue("@fromUser", transfer.AccountFrom.UserId);
                cmd.Parameters.AddWithValue("@fromUserBalance", transfer.Amount);
                cmd.ExecuteNonQuery();
                //Update the To User 
                cmd = new SqlCommand(sqlUpdateTo, conn);
                cmd.Parameters.AddWithValue("@toUser", transfer.AccountTo.UserId);
                cmd.Parameters.AddWithValue("@toUserBalance", transfer.Amount);
                cmd.ExecuteNonQuery();


            }
        }

        public Transfer CreateTransfer(Transfer transfer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                const string sql = @"INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount)
                                        VALUES (@transfer_type_id, @transfer_status_id, @account_from, @account_to, @amount)
                                        SELECT @@Identity";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@transfer_type_id", (int)transfer.TransferType);
                cmd.Parameters.AddWithValue("@transfer_status_id", (int)transfer.TransferStatus);
                cmd.Parameters.AddWithValue("@account_from", transfer.AccountFrom.UserId);
                cmd.Parameters.AddWithValue("@account_to", transfer.AccountTo.UserId);
                cmd.Parameters.AddWithValue("@amount", transfer.Amount);

                transfer.TransferId = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return transfer;
        }

        public ReturnUser GetUserById(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                ReturnUser user = new ReturnUser();
                conn.Open();

                string sqlQuery = @"SELECT * from users where user_id = @userId";
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@userid", id);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    user = new ReturnUser();

                    user.UserId = Convert.ToInt32(rdr["user_id"]);
                    user.Username = Convert.ToString(rdr["username"]);
                }
                return user;
            }
        }
    }
}
