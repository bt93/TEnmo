using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using RestSharp.Authenticators;
using TenmoServer.Models;
using System.Net;

namespace TenmoClient
{
    public class ApiService
    {
        private const string API_URL = "https://localhost:44315/";
        private RestClient client = new RestClient();
        private string token;

        public ApiService()
        {
            token = UserService.GetToken();
            client.Authenticator = new JwtAuthenticator(token);
        }

        public Account GetBalance()
        {
            RestRequest request = new RestRequest($"{API_URL}accounts/balance");
            IRestResponse<Account> response = client.Get<Account>(request);
            ErrorHandling(response);
            return response.Data;
        }

        public List<Transfer> GetTransfers()
        {
            RestRequest request = new RestRequest($"{API_URL}accounts/transfers");
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);
            ErrorHandling(response);
            return response.Data;
        }
        public Transfer GetTransferById(int id)
        {
            RestRequest request = new RestRequest($"{API_URL}accounts/transfers/{id}");
            IRestResponse<Transfer> response = client.Get<Transfer>(request);
            ErrorHandling(response);
            return response.Data;
        }

        public List<ReturnUser> GetAllUsers()
        {
            RestRequest request = new RestRequest($"{API_URL}accounts");
            IRestResponse<List<ReturnUser>> response = client.Get<List<ReturnUser>>(request);
            ErrorHandling(response);
            return response.Data;
        }

        public Transfer SendTEBucks(Transfer transfer)
        {
            RestRequest request = new RestRequest($"{API_URL}accounts/transfers");
            request.AddJsonBody(transfer);
            IRestResponse<Transfer> response = client.Post<Transfer>(request);
            ErrorHandling(response);
            return response.Data;
        }

        public Transfer RequestTransfer(Transfer transfer)
        {
            RestRequest request = new RestRequest($"{API_URL}accounts/transfers/request");
            request.AddJsonBody(transfer);
            IRestResponse<Transfer> response = client.Post<Transfer>(request);
            ErrorHandling(response);
            return response.Data;
        }

        public void ErrorHandling(IRestResponse resp)
        {
            if (resp.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("Error occurred - unable to reach server.");
            }
            else if (!resp.IsSuccessful)
            {
                if (resp.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new Exception("Access denied: you do not have permission for viewing this data.");
                }
                else if (resp.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new Exception("Sorry, the data you are looking for cannot be found.");
                }
                else
                {
                    throw new Exception("Error occurred - received non-success response: " + (int)resp.StatusCode);
                }               
            }
        }
    }
}
