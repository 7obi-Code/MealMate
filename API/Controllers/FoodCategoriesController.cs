
using DataAccess.DTOs;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class FoodCategoriesController : ControllerBase
{
    private readonly IFFoodCategoryDao _categoryDao;

    public FoodCategoriesController(IFFoodCategoryDao categoryDao)
    {
        _categoryDao = categoryDao;
    }

    [HttpPost]
    public IActionResult CreateCategory(CreateFoodCategoryRequest request)
    {
        if (request.MenuId <= 0 || string.IsNullOrWhiteSpace(request.Name))
            return BadRequest();

        int categoryId = _categoryDao.CreateCategory(
            request.MenuId,
            request.Name,
            request.FoodItemIds);

        return Ok(categoryId);
    }
}
