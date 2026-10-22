
using APIClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Website.Pages.Restaurant
{
    public class IndexModel : PageModel
    {
        private readonly RestaurantApiClient _restaurantApiClient;

        public DataAccess.Model.Restaurant? Restaurant { get; private set; }

        public IndexModel(RestaurantApiClient restaurantApiClient)
        {
            _restaurantApiClient = restaurantApiClient;
        }

        public IActionResult OnGet(int id)
        {
            Restaurant = _restaurantApiClient.GetRestaurantMenuById(id);

            if (Restaurant == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
