
using DataAccess.Model;
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
    }
}
