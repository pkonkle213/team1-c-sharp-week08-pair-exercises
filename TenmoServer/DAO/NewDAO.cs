using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.DAO
{
    public class NewDAO
    {
        private readonly string connectionString;
        
        public NewDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public decimal GetUserBalance(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT balance FROM accounts WHERE user_id = @user_id", conn);
                cmd.Parameters.AddWithValue("@user_id", id);
                return Convert.ToDecimal(cmd.ExecuteScalar());
            }
        }
    }
}
