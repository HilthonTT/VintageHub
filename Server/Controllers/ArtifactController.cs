namespace VintageHub.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes")]
public class ArtifactController : ControllerBase
{
    private readonly IArtifactData _artifactData;
    private readonly ILogger<ArtifactController> _logger;

    public ArtifactController(
        IArtifactData artifactData,
        ILogger<ArtifactController> logger)
    {
        _artifactData = artifactData;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<ArtifactDisplayModel>>> GetAllArtifactsAsync()
    {
        try
        {
            var artifacts = await _artifactData.GetAllArtifactsAsync();

            return Ok(artifacts);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching artifacts: {error}", ex.Message);
            return StatusCode(500, "Error fetching artifacts.");
        }
    }

    [HttpGet("vendor/{id}")]
    public async Task<ActionResult<List<ArtifactDisplayModel>>> GetArtifactByVendorIdAsync(int id)
    {
        try
        {
            var artifacts = await _artifactData.GetAllArtifactsByVendorIdAsync(id);

            return Ok(artifacts);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching artifact: {error}", ex.Message);
            return StatusCode(500, $"Error fetching artifact by vendor id of {id}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ArtifactDisplayModel>> GetArtifactByIdAsync(int id)
    {
        try
        {
            var artifact = await _artifactData.GetArtifactByIdAsync(id);

            return Ok(artifact);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching artifact: {error}", ex.Message);
            return StatusCode(500, $"Error fetching artifact by id of {id}");
        }
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<ArtifactDisplayModel>> InsertArtifactAsync([FromBody] ArtifactModel artifact)
    {
        if (ModelState.IsValid is false)
        {
            return BadRequest(ModelState);
        }

        try
        {
            int createdArtifactId = await _artifactData.InsertArtifactAsync(artifact);
            var createdArtifact = await _artifactData.GetArtifactByIdAsync(createdArtifactId);

            return Ok(createdArtifact);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error inserting artifact: {error}", ex.Message);
            return StatusCode(500, $"Error inserting the artifact.");
        }
    }

    [HttpPut]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult> UpdateArtifactAsync([FromBody] ArtifactModel artifact)
    {
        if (ModelState.IsValid is false)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _artifactData.InsertArtifactAsync(artifact);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error updating artifact: {error}", ex.Message);
            return StatusCode(500, $"Error updating the artifact.");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult> DeleteArtifactAsync(int id)
    {
        if (ModelState.IsValid is false)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _artifactData.DeleteArtifactAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting artifact: {error}", ex.Message);
            return StatusCode(500, $"Error deleting the artifact.");
        }
    }
}
