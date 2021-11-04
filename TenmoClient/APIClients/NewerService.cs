using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;
using TenmoClient.Models;

namespace TenmoClient.APIClients
{
    public class NewerService
    {
        private const string API_BASE_URL = "https://localhost:44315/";
        private readonly IRestClient client = new RestClient();

        public bool TransferFunds(int destinationId, decimal amount, string token)
        {
            RestRequest request = new RestRequest(API_BASE_URL + "user/transfers");
            request.AddHeader("Authorization", "bearer " + token);
            
            //Come back to this
            
            return false;
        }
    }
}
