﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class UserInfoDAO : IUserInfoDAO
    {
        private readonly string connectionString;

        public UserInfoDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<User> AllUsers(int id)
        {
            List<User> users = new List<User>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT user_id, username " +
                    "FROM users " +
                    "WHERE user_id != @user_id", conn);
                cmd.Parameters.AddWithValue("@user_id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    User user = BuildUserFromReader(reader);
                    users.Add(user);
                }
            }
            return users;
        }

        private User BuildUserFromReader(SqlDataReader reader)
        {
            User user = new User();
            user.UserId = Convert.ToInt32(reader["user_id"]);
            user.Username = Convert.ToString(reader["username"]);

            return user;
        }
    }
}