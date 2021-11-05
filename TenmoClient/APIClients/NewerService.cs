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

        public void TransferFunds(int destinationId, decimal amount, string token)
        {
            RestRequest request = new RestRequest(API_BASE_URL + $"transfer?destinationId={destinationId}&amount={amount}");
            request.AddHeader("Authorization", "bearer " + token);
            IRestResponse response = client.Put(request);

            //Test to see how the response returned to give an error message
        }

        public List<Transfer> AllTransfers(string token)
        {
            RestRequest request = new RestRequest(API_BASE_URL + "transfer");
            request.AddHeader("Authorization", "bearer " + token);
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);
            return response.Data;
        }

        public Transfer SpecificTransfer(int transferId, string token)
        {
            RestRequest request = new RestRequest(API_BASE_URL + $"transfer/{transferId}");
            request.AddHeader("Authorization", "bearer " + token);
            IRestResponse<Transfer> response = client.Get<Transfer>(request);
            return response.Data;
        }
    }
}
