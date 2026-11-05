using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using DataAccess.Model;

namespace APIClient
{
    public class CustomerApiClient(string restUrl)
    {
        private readonly RestClient _client = new RestClient(restUrl);

        // Sender en POST-request til API'et for at oprette en kunde
        public int? CreateCustomer(Customer customer)
        {
            var request = new RestRequest("", Method.Post);

            // Sender Customer-objektet som JSON i request body
            request.AddJsonBody(customer);

            var response = _client.Execute<int>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            return null;
        }

        // Sender en GET-request til API'et for at hente en kunde
        public Customer? GetCustomerById(int customerId)
        {
            var request = new RestRequest($"{customerId}", Method.Get);

            var response = _client.Execute<Customer>(request);

            if (response.IsSuccessful && response.Data != null)
            {
                return response.Data;
            }

            return null;
        }
    }
}
    
