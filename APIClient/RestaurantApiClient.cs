using DataAccess.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace APIClient
{
    public class RestaurantApiClient(string restUrl)
    {

        RestClient _client = new RestClient(restUrl);

        public Restaurant? GetRestaurantMenuById(int restaurantId)
        {
            var request = new RestRequest($"{restaurantId}");

            var response = _client.Execute<Restaurant>(request);

            if (response.IsSuccessful && response.Data != null)
            {
                return response.Data;
            }

            return null;
        }

    }
}
