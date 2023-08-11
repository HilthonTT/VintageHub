using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Server.Library.Models;

namespace VintageHub.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes")]
public class ReviewController : ControllerBase
{
    private readonly IReviewData _reviewData;
    private readonly ILogger<ReviewController> _logger;

    public ReviewController(
        IReviewData reviewData,
        ILogger<ReviewController> logger)
    {
        _reviewData = reviewData;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReviewModel>> GetReviewByIdAsync(int id)
    {
        try
        {
            var review = await _reviewData.GetReviewByIdAsync(id);

            if (review is null)
            {
                return NotFound("Review Not Found.");
            }

            return Ok(review);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching reviews: {error}", ex.Message);
            return StatusCode(500, $"Error fetching reviews of id {id}");
        }
    }

    [HttpGet("artifact/{artifactId}")]
    public async Task<ActionResult<List<ReviewModel>>> GetReviewsByArtifactId(int artifactId)
    {
        try
        {
            var reviews = await _reviewData.GetReviewsByArtifactIdAsync(artifactId);
            return Ok(reviews);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching reviews: {error}", ex.Message);
            return StatusCode(500, $"Error fetching reviews of artifact id {artifactId}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<ReviewModel>> InsertReviewAsync([FromBody] ReviewModel review)
    {
        if (ModelState.IsValid is false)
        {
            return BadRequest(ModelState);
        }

        try
        {
            int createdReviewId = await _reviewData.InsertReviewAsync(review);
            return CreatedAtAction(nameof(GetReviewByIdAsync), new { id = createdReviewId }, review);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error inserting the review: {error}", ex.Message);
            return StatusCode(500, "Error inserting the review.");
        }
    }

    [HttpPut]
    public async Task<ActionResult> UpdateReviewAsync([FromBody] ReviewModel review)
    {
        if (ModelState.IsValid is false)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _reviewData.UpdateReviewAsync(review);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error updating the review: {error}", ex.Message);
            return StatusCode(500, "Error updating the review.");
        }
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteReviewAsync([FromBody] ReviewModel review)
    {
        if (ModelState.IsValid is false)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _reviewData.DeleteReviewAsync(review);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error delete the review: {error}", ex.Message);
            return StatusCode(500, "Error delete the review.");
        }
    }
}
