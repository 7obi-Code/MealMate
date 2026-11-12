using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.DTOs;
using RestSharp;

namespace APIClient;

public class FoodCategoryApiClient(string categoryUrl)
{
    private readonly RestClient _client = new(categoryUrl);

    public bool CreateCategory(CreateFoodCategoryRequest request)
    {
        var restRequest = new RestRequest("", Method.Post);
        restRequest.AddJsonBody(request);

        var response = _client.Execute<int>(restRequest);
        return response.IsSuccessful;
    }

    public bool UpdateCategory(
    int categoryId,
    CreateFoodCategoryRequest request)
    {
        var apiRequest = new RestRequest(
            $"{categoryId}",
            Method.Put
        );

        apiRequest.AddJsonBody(request);

        var response = _client.Execute(apiRequest);

        return response.IsSuccessful;
    }

    public bool DeleteCategory(int categoryId)
    {
        var request = new RestRequest(
            $"{categoryId}",
            Method.Delete
        );

        var response = _client.Execute(request);

        return response.IsSuccessful;
    }
}

