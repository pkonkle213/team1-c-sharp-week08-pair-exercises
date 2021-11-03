using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;

namespace TenmoClient.APIClients
{
    public class NewService
    {
        private const string API_BASE_URL = "https://localhost:44315/";
        private readonly IRestClient client = new RestClient();
        
        public decimal Balance(string token)
        {
            RestRequest request = new RestRequest(API_BASE_URL + "user/balance");
            request.AddHeader("Authorization", token);
            IRestResponse<decimal> response = client.Get<decimal>(request);

            return response.Data;
        }
    }
}
