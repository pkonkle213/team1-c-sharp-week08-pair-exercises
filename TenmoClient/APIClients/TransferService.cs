using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;
using TenmoClient.Models;

namespace TenmoClient.APIClients
{
    public class TransferService
    {
        private const string API_BASE_URL = "https://localhost:44315/";
        private readonly IRestClient client = new RestClient();

        public void TransferFunds(int destinationId, decimal amount, string token)
        {
            Transfer transfer = new Transfer();
            transfer.TransferAmount = amount;
            transfer.ReceiverId = destinationId;
            RestRequest request = new RestRequest(API_BASE_URL + "money");
            request.AddHeader("Authorization", "bearer " + token);
            request.AddJsonBody(transfer);
            IRestResponse response = client.Put(request);
        }

        public List<Transfer> AllTransfers(string token)
        {
            RestRequest request = new RestRequest(API_BASE_URL + "money");
            request.AddHeader("Authorization", "bearer " + token);
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);
            return response.Data;
        }

        public Transfer SpecificTransfer(int transferId, string token)
        {
            RestRequest request = new RestRequest(API_BASE_URL + $"money/{transferId}");
            request.AddHeader("Authorization", "bearer " + token);
            IRestResponse<Transfer> response = client.Get<Transfer>(request);
            return response.Data;
        }
    }
}
