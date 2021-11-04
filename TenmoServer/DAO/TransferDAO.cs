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
        public bool TransferFunds(int userId, int destinationId, decimal transferAmount)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO transfers (transfer_type_id,transfer_status_id,account_from,account_to,amount) VALUES(2, 2, @userId, @receiverId, @amountTransferred)", conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@receiverId", destinationId);
                cmd.Parameters.AddWithValue("@amountTransferred", transferAmount);
                cmd.ExecuteNonQuery();

                return true;
                //Does this need to be bool or should it be void?
            }
        }
    }
}
