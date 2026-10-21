using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Website.Pages.Restaurant
{
    public class IndexModel : PageModel
    {
        public int RestaurantId { get; private set; }

        public IActionResult OnGet(int id)
        {
            RestaurantId = id;
            return Page();
        }
    }
}