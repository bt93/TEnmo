﻿using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using RestSharp.Authenticators;
using TenmoServer.Models;

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

            return response.Data;
        }
    }
}
