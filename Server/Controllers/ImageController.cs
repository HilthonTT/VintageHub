namespace VintageHub.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes")]
public class ImageController : ControllerBase
{
    private const int MaxFileSize = 1024 * 1024 * 5; // represents 5MB
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
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<string>> UploadImageAsync(IFormFile imageFile)
    {
        try
        {
            if (imageFile is null || imageFile.Length == 0)
            {
                return BadRequest("Image file is required.");
            }

            if (imageFile.Length > MaxFileSize)
            {
                return BadRequest("Image Size is too large.");
            }

            using var imageStream = imageFile.OpenReadStream();
            string objectId = await _imageData.UploadImageAsync(imageStream);

            return Ok(objectId);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error uploading the image: {error}", ex.Message);
            return StatusCode(500, "Error uploading the image.");
        }
    }

    [HttpGet("{objectId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetImage(string objectId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(objectId))
            {
                return BadRequest("The Object Identifier must be provided.");
            }

            byte[] imageData = await _imageData.GetImageAsync(objectId);
            if (imageData is null)
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync("https://dummyimage.com/600x400/000/fff");

                if (response.IsSuccessStatusCode)
                {
                    imageData = await response.Content.ReadAsByteArrayAsync();
                }
                else
                {
                    return StatusCode(500, "Error fetching default image.");
                }
            }

            Response.Headers.Add("Content-Type", "image/jpeg");

            return File(imageData, "image/jpeg");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching the image: {error}", ex.Message);
            return StatusCode(500, "Error fetching the image.");
        }
    }

    [HttpDelete("{objectId}")]
    [Authorize(Policy = "Admin")]
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
