using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Server.Library.Models;

namespace VintageHub.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryData _categoryData;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(
        ICategoryData categoryData,
        ILogger<CategoryController> logger)
    {
        _categoryData = categoryData;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryModel>>> GetAllCategoriesAsync()
    {
        try
        {
            var artifacts = await _categoryData.GetAllCategoriesAsync();
            return Ok(artifacts);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching the categories: {error}", ex.Message);
            return StatusCode(500, "Error fetching the categories.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryModel>> GetCategoryByIdAsync(int id)
    {
        try
        {
            var artifact = await _categoryData.GetCategoryByIdAsync(id);

            return Ok(artifact);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching the category: {error}", ex.Message);
            return StatusCode(500, $"Error fetching the category by id of {id}");
        }
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<CategoryModel>> InsertCategoryAsync([FromBody] CategoryModel category)
    {
        if (ModelState.IsValid is false)
        {
            return BadRequest(ModelState);
        }

        try
        {
            int createdCategoryId = await _categoryData.InsertCategoryAsync(category);

            var createdCategory = await _categoryData.GetCategoryByIdAsync(createdCategoryId);
            return Ok(createdCategory);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error inserting the category: {error}", ex.Message);
            return StatusCode(500, "Error inserting the category.");
        }
    }

    [HttpPut]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult> UpdateCategoryAsync([FromBody] CategoryModel category)
    {
        try
        {
            await _categoryData.UpdateCategoryAsync(category);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error updating category: {error}", ex.Message);
            return StatusCode(500, "Error updating the category.");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult> DeleteCategoryAsync(int id)
    {
        try
        {
            await _categoryData.DeleteCategoryAsync(id);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting category: {error}", ex.Message);
            return StatusCode(500, "Error deleting category.");
        }
    }
}
