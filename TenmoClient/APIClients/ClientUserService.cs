using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;
using TenmoClient.Models;

namespace TenmoClient.APIClients
{
    public class ClientUserService
    {
        private const string API_BASE_URL = "https://localhost:44315/";
        private readonly IRestClient client = new RestClient();

        public decimal Balance(string token)
        {
            RestRequest request = new RestRequest(API_BASE_URL + "money/balance");
            request.AddHeader("Authorization", "bearer " + token);
            IRestResponse<decimal> response = client.Get<decimal>(request);
            return response.Data;
        }

        public List<User> AllUsers(string token)
        {
            RestRequest request = new RestRequest(API_BASE_URL + "user");
            request.AddHeader("Authorization", "bearer " + token);
            IRestResponse<List<User>> response = client.Get<List<User>>(request);
            return response.Data;
        }
    }
}
