
using DataAccess.Model;
using DataAccess.DTOs;
using RestSharp;
using System.Collections.Generic;

namespace APIClient
{
    public class RestaurantApiClient(string restUrl)
    {
        private readonly RestClient _client = new RestClient(restUrl);

        public List<Restaurant> GetAllRestaurants()
        {
            var request = new RestRequest("", Method.Get);

            var response = _client.Execute<List<Restaurant>>(request);

            if (response.IsSuccessful && response.Data != null)
            {
                return response.Data;
            }

            return new List<Restaurant>();
        }

        public Restaurant? GetRestaurantMenuById(int restaurantId)
        {
            var request = new RestRequest($"{restaurantId}", Method.Get);

            var response = _client.Execute<Restaurant>(request);

            if (response.IsSuccessful && response.Data != null)
            {
                return response.Data;
            }

            return null;
        }
        public bool UpdateOnlineSales(int restaurantId, bool isAcceptingOrders)
        {
            var request = new RestRequest(
                $"{restaurantId}/OnlineSales",
                Method.Put
            );

            request.AddParameter(
                "application/json",
                isAcceptingOrders.ToString().ToLower(),
                ParameterType.RequestBody
            );

            var response = _client.Execute(request);

            return response.IsSuccessful;
        }
    }
}
