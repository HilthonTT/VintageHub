using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace VintageHub.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes")]
public class ImageController : ControllerBase
{
    private readonly IImageData _imageData;
    private readonly ILogger<ImageController> _logger;

    public ImageController(
        IImageData imageData,
        ILogger<ImageController> logger)
    {
        _imageData = imageData;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<string>> UploadImageAsync(IFormFile imageFile)
    {
        try
        {
            if (imageFile is null || imageFile.Length == 0)
            {
                return BadRequest("Image file is required.");
            }

            string objectId = await _imageData.UploadImageAsync(imageFile);

            return Ok(objectId);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error uploading the image: {error}", ex.Message);
            return StatusCode(500, "Error uploading the image.");
        }
    }

    [HttpGet("{objectId}")]
    public async Task<ActionResult<byte[]>> GetImageAsync(string objectId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(objectId))
            {
                return BadRequest("The Object Identifier must be provided.");
            }

            return await _imageData.GetImageAsync(objectId);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching the image: {error}", ex.Message);
            return StatusCode(500, "Error fetching the image.");
        }
    }

    [HttpDelete("{objectId}")]
    public async Task<ActionResult> DeleteImageAsync(string objectId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(objectId))
            {
                return BadRequest("The Object Identifier must be provided.");
            }

            await _imageData.DeleteImageAsync(objectId);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting the image: {error}", ex.Message);
            return StatusCode(500, "Error deleting the image.");
        }
    }
}
