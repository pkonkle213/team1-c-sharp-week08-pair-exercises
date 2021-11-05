using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class MoneyDAO : IMoneyDAO
    {
        private readonly string connectionString;

        public MoneyDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public void Transfer(int userId, int destinationId, decimal transferAmount)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                int accountSend = FindAccount(userId, conn);
                int accountRec = FindAccount(destinationId, conn);
                ExecuteTransfer(transferAmount, conn, accountSend, accountRec);
                UpdateBalances(userId, destinationId, transferAmount, conn);
            }
        }

        public decimal UserBalance(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT balance " +
                    "FROM accounts " +
                    "WHERE user_id = @user_id", conn);
                cmd.Parameters.AddWithValue("@user_id", id);
                return Convert.ToDecimal(cmd.ExecuteScalar());
            }
        }

        private static int FindAccount(int userId, SqlConnection conn)
        {
            SqlCommand findAccount = new SqlCommand("SELECT a.account_id, a.balance " +
                "FROM accounts a " +
                "INNER JOIN users u ON a.user_id = u.user_id " +
                "WHERE u.user_id = @userId", conn);
            findAccount.Parameters.AddWithValue("@userId", userId);
            int account = Convert.ToInt32(findAccount.ExecuteScalar());
            return account;
        }
        
        private static void ExecuteTransfer(decimal transferAmount, SqlConnection conn, int accountSend, int accountRec)
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO transfers (transfer_type_id,transfer_status_id,account_from,account_to,amount) " +
                "VALUES(1001, 2001, @userId, @receiverId, @amountTransferred)", conn);
            cmd.Parameters.AddWithValue("@userId", accountSend);
            cmd.Parameters.AddWithValue("@receiverId", accountRec);
            cmd.Parameters.AddWithValue("@amountTransferred", transferAmount);
            cmd.ExecuteNonQuery();
        }

        private static void UpdateBalances(int userId, int destinationId, decimal transferAmount, SqlConnection conn)
        {
            SqlCommand cmd2 = new SqlCommand("UPDATE accounts " +
                "SET balance += @transferamount " +
                "WHERE user_id = @recipientid; " +
                "UPDATE accounts " +
                "SET balance -= @transferamount " +
                "WHERE user_id = @senderid", conn);
            cmd2.Parameters.AddWithValue("@transferamount", transferAmount);
            cmd2.Parameters.AddWithValue("@recipientid", destinationId);
            cmd2.Parameters.AddWithValue("@senderid", userId);
            cmd2.ExecuteNonQuery();
        }

        public List<Transfer> AllTransfers(int userId)
        {
            List<Transfer> allTransfers = new List<Transfer>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT t.transfer_id AS TransferID,a.user_id AS SenderID,u.username AS SenderName,s.username AS ReceiverName,t.amount AS TransferAmount " +
                    "FROM transfers t " +
                    "INNER JOIN accounts a ON a.account_id = t.account_from " +
                    "INNER JOIN accounts b ON b.account_id = t.account_to " +
                    "INNER JOIN users u ON u.user_id = a.user_id " +
                    "INNER JOIN users s ON s.user_id = b.user_id " +
                    "WHERE a.user_id = @userId OR b.user_id = @userId", conn);

                cmd.Parameters.AddWithValue("@userId", userId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Transfer transfer = BuildTransferFromReader(reader, userId);
                    allTransfers.Add(transfer);
                }
            }
            return allTransfers;
        }

        public Transfer SpecificTransfer(int transferId)
        {
            Transfer transfer = new Transfer();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT t.transfer_id AS TransferID,ts.transfer_status_desc AS TransferStatus,tt.transfer_type_desc AS TransferType,u.username AS SenderName,s.username AS ReceiverName,t.amount AS TransferAmount " +
                    "FROM transfers t " +
                    "INNER JOIN accounts a ON a.account_id = t.account_from " +
                    "INNER JOIN accounts b ON b.account_id = t.account_to " +
                    "INNER JOIN users u ON u.user_id = a.user_id " +
                    "INNER JOIN users s ON s.user_id = b.user_id " +
                    "INNER JOIN transfer_types tt ON tt.transfer_type_id = t.transfer_type_id " +
                    "INNER JOIN transfer_statuses ts ON ts.transfer_status_id = t.transfer_status_id " +
                    "WHERE t.transfer_id = @transferid", conn);

                cmd.Parameters.AddWithValue("@transferid", transferId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    transfer = BuildOneTransferFromReader(reader);
                }       
            }

            return transfer;
        }

        private Transfer BuildOneTransferFromReader(SqlDataReader reader)
        {
            Transfer transfer = new Transfer();
            transfer.TransferId = Convert.ToInt32(reader["TransferID"]);
            transfer.TransferAmount = Convert.ToDecimal(reader["TransferAmount"]);
            transfer.SenderName = Convert.ToString(reader["SenderName"]);
            transfer.TransferStatus = Convert.ToString(reader["TransferStatus"]);
            transfer.TransferType = Convert.ToString(reader["TransferType"]);
            transfer.ReceiverName = Convert.ToString(reader["ReceiverName"]);
            return transfer;
        }

        private Transfer BuildTransferFromReader(SqlDataReader reader, int userId)
        {
            Transfer transfer = new Transfer();
            transfer.TransferId = Convert.ToInt32(reader["TransferID"]);
            transfer.TransferAmount = Convert.ToDecimal(reader["TransferAmount"]);

            int test = Convert.ToInt32(reader["SenderID"]);
            if (test == userId)
            {
                transfer.Direction = "To: ";
                transfer.Username = Convert.ToString(reader["ReceiverName"]);
            }
            else
            {
                transfer.Direction = "From: ";
                transfer.Username = Convert.ToString(reader["SenderName"]);
            }
            return transfer;
        }
    }
}
