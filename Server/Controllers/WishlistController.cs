﻿namespace VintageHub.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class WishlistController : ControllerBase
{
    private readonly IWishlistData _wishlistData;
    private readonly ILogger<WishlistController> _logger;

    public WishlistController(
        IWishlistData wishlistData,
        ILogger<WishlistController> logger)
    {
        _wishlistData = wishlistData;
        _logger = logger;
    }

    [HttpGet("artifacts/{userId}")]
    public async Task<ActionResult<List<ArtifactDisplayModel>>> GetAllArtifactsInWishlistAsync(int userId)
    {
        try
        {
            var artifacts = await _wishlistData.GetAllArtifactsInWishlistAsync(userId);
            return Ok(artifacts);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching the artifacts in the wishlist: {error}", ex.Message);
            return StatusCode(500, "Error fetching artifacts in wishlist.");
        }
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<List<WishlistModel>>> GetAllWishlistAsyncByUserIdAsync(int userId)
    {
        try
        {
            var wishlists = await _wishlistData.GetAllWishlistsByUserIdAsync(userId);
            return Ok(wishlists);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching the wishlists: {error}", ex.Message);
            return StatusCode(500, $"Error fetching wishlists of user id {userId}.");
        }
    }

    [HttpPost]
    public async Task<ActionResult<List<ArtifactDisplayModel>>> InsertWishlistAsync([FromBody] WishlistModel wishlist)
    {
        if (ModelState.IsValid is false)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _wishlistData.InsertWishlistAsync(wishlist);
            var artifacts = await _wishlistData.GetAllArtifactsInWishlistAsync(wishlist.UserId);

            return Ok(artifacts);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error inserting the wishlist: {error}", ex.Message);
            return StatusCode(500, "Error inserting the wishlist.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteWishlistAsync(int id)
    {
        try
        {
            await _wishlistData.DeleteWishlistAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting the wishlist: {error}", ex.Message);
            return StatusCode(500, $"Error deleting the wishlist.");
        }
    }
}
