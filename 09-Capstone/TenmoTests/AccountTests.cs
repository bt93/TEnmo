using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using TenmoServer.DAO;
using TenmoServer.Models;
using TenmoServer.Security;
using TenmoServer.Controllers;
using RestSharp;
using RestSharp.Authenticators;
using TenmoServer;
using System.Collections.Generic;

namespace TenmoTests
{
    [TestClass]
    public class AccountTests
    {

        // TODO: Come back to create tests

        [TestMethod]
        public void NewUserAccountBalanceIs1000()
        {
            //Arrange
            string API_URL_LOGIN = "https://localhost:44315/login";
            string API_URL = "https://localhost:44315/accounts";
            LoginUser user = new LoginUser();
            user.Username = "test";
            user.Password = "test";



            //string connectionString = "Server=.\\SQLEXPRESS;Database=tenmo;Trusted_Connection=True;";
            
            RestClient client = new RestClient();

            // Still Arranging
            // Login to get credentials
            RestRequest request = new RestRequest(API_URL_LOGIN);
            request.AddJsonBody(user);
            IRestResponse<ReturnUser> response = client.Post<ReturnUser>(request);
            ReturnUser authenticatedUser = response.Data;
            client.Authenticator = new JwtAuthenticator(authenticatedUser.Token);

            // get highest Id
            request = new RestRequest(API_URL + "/transfers");
            IRestResponse<List<ReturnUser>> responseUsers = client.Get<List<ReturnUser>>(request);
            List<ReturnUser> users = responseUsers.Data;
            int highestId = 0;
            foreach (ReturnUser returnUser in users)
            {
                if (returnUser.UserId > highestId)
                {
                    highestId = returnUser.UserId;
                }
            }

            // Register new user   
            highestId++;
            LoginUser testUser = new LoginUser();
            testUser.Username = $"test{highestId}";
            testUser.Password = "test";
            RestClient registerClient = new RestClient();
            RestRequest registerRequest = new RestRequest(API_URL_LOGIN + "/register");
            registerRequest.AddJsonBody(testUser);
            IRestResponse registerResponse = registerClient.Post(registerRequest);

            // Login with testUser
            registerRequest = new RestRequest(API_URL_LOGIN);
            registerRequest.AddJsonBody(testUser);
            IRestResponse<ReturnUser> loginResponse = registerClient.Post<ReturnUser>(registerRequest);
            ReturnUser returnTestUser = loginResponse.Data;
            registerClient.Authenticator = new JwtAuthenticator(returnTestUser.Token);

            // Act
            // assert Balance is 1000
            registerRequest = new RestRequest(API_URL + "/balance");
            IRestResponse<Account> balanceResponse = registerClient.Get<Account>(registerRequest);
            Account account = balanceResponse.Data;

            // Assert
            Assert.AreEqual(1000M, account.Balance);




            /**
             * The problem here is that the database is not testable
             * If I hit the controller with tests, the db is not returned to the same state for testing purposes
             * Is there a way to override Startup of TenmoServer to tenmotestbd?
             * We could change appsetting.json to point to testdb
             * 
             * The tests could also target the existing db using rolled back transactions and performing transfers between known entities
             * 
             * The process would be like such:
             * register new client, hold token for authentication, Assert balance, rollback
             * register two new clients and authenticate each, make transfer, Assert balances, rollback
             * register three new clients, make a complex transfer, Assert balances, rollback
             * ...
             * 
             **/
            


        }
    }
}
