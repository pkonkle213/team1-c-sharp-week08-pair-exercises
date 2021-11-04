using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.DAO
{
    public class TransferDAO : ITransferDAO
    {
        private readonly string connectionString;

        public TransferDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public void Transfer(int userId, int destinationId, decimal transferAmount)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand findAccountA = new SqlCommand("SELECT a.account_id FROM accounts a INNER JOIN users u ON a.user_id = u.user_id WHERE u.user_id = @userId", conn);
                findAccountA.Parameters.AddWithValue("@userId", userId);
                int accountSend = Convert.ToInt32(findAccountA.ExecuteScalar());

                SqlCommand findAccountB = new SqlCommand("SELECT a.account_id FROM accounts a INNER JOIN users u ON a.user_id = u.user_id WHERE u.user_id = @recipient", conn);
                findAccountB.Parameters.AddWithValue("@recipient", destinationId);
                int accountRec = Convert.ToInt32(findAccountB.ExecuteScalar());

                SqlCommand cmd = new SqlCommand("INSERT INTO transfers (transfer_type_id,transfer_status_id,account_from,account_to,amount) VALUES(1001, 2001, @userId, @receiverId, @amountTransferred)", conn);
                cmd.Parameters.AddWithValue("@userId", accountSend);
                cmd.Parameters.AddWithValue("@receiverId", accountRec);
                cmd.Parameters.AddWithValue("@amountTransferred", transferAmount);
                cmd.ExecuteNonQuery();

                SqlCommand cmd2 = new SqlCommand("UPDATE accounts SET balance += @transferamount WHERE user_id = @recipientid; UPDATE accounts SET balance -= @transferamount WHERE user_id = @senderid", conn);
                cmd2.Parameters.AddWithValue("@transferamount", transferAmount);
                cmd2.Parameters.AddWithValue("@recipientid", destinationId);
                cmd2.Parameters.AddWithValue("@senderid", userId);
                cmd2.ExecuteNonQuery();
            }
        }
    }
}
