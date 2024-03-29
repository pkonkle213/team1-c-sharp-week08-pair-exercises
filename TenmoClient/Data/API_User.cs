﻿namespace TenmoClient.Data
{
    /// <summary>
    /// Return value from login endpoint
    /// </summary>
    public class API_User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public decimal Balance { get; set; }
    }
}
