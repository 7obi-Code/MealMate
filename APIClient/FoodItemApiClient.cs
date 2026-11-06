
using DataAccess.DTOs;
using RestSharp;

namespace APIClient
{
    public class FoodItemApiClient(string foodItemUrl)
    {
        private readonly RestClient _client = new RestClient(foodItemUrl);

        //Manager Funktion
        public int? CreateFoodItem(CreateFoodItemRequest foodItem)
        {
            var request = new RestRequest("", Method.Post);
            request.AddJsonBody(foodItem);

            var response = _client.Execute<int>(request);

            return response.IsSuccessful ? response.Data : null;
        }

        //Manager Funktion
        public bool UpdateFoodItem(int foodItemId, UpdateFoodItemRequest foodItem)
        {
            var request = new RestRequest($"{foodItemId}", Method.Put);
            request.AddJsonBody(foodItem);

            var response = _client.Execute(request);

            return response.IsSuccessful;
        }

        //Manager Funktion
        public bool DeleteFoodItem(int foodItemId)
        {
            var request = new RestRequest($"{foodItemId}", Method.Delete);

            var response = _client.Execute(request);

            return response.IsSuccessful;
        }
    }
}
