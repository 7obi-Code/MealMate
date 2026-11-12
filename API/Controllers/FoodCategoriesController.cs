
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

    [HttpPut("{categoryId}")]
    public IActionResult UpdateCategory(
        int categoryId,
        [FromBody] CreateFoodCategoryRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest();

        bool updated = _categoryDao.UpdateCategory(
            categoryId,
            request.MenuId,
            request.Name,
            request.FoodItemIds);

        if (!updated)
            return NotFound();

        return Ok();
    }

    [HttpDelete("{categoryId}")]
    public IActionResult DeleteCategory(int categoryId)
    {
        bool deleted = _categoryDao.DeleteCategory(categoryId);

        if (!deleted)
            return NotFound();

        return Ok();
    }
}
