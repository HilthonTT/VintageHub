﻿namespace VintageHub.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes")]
public class EraController : ControllerBase
{
    private readonly IEraData _eraData;
    private readonly ILogger<EraController> _logger;

    public EraController(
        IEraData eraData,
        ILogger<EraController> logger)
    {
        _eraData = eraData;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<EraModel>>> GetAllErasAsync()
    {
        try
        {
            var eras = await _eraData.GetAllErasAsync();
            return Ok(eras);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching the eras: {error}", ex.Message);
            return StatusCode(500, "Error fetching the eras");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EraModel>> GetEraByIdAsync(int id)
    {
        try
        {
            var era = await _eraData.GetEraByIdAsync(id);

            return Ok(era);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching the era: {error}", ex.Message);
            return StatusCode(500, $"Error fetching the era of id {id}");
        }
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<EraModel>> InsertEraAsync([FromBody] EraModel era)
    {
        if (ModelState.IsValid is false)
        {
            return BadRequest(ModelState);
        }

        try
        {
            int createdEraId = await _eraData.InsertEraAsync(era);
            var createdEra = await _eraData.GetEraByIdAsync(createdEraId);
            
            return Ok(createdEra);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error inserting the era: {error}", ex.Message);
            return StatusCode(500, "Error inserting the era.");
        }
    }

    [HttpPut]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult> UpdateEraAsync([FromBody] EraModel era)
    {
        if (ModelState.IsValid is false)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _eraData.UpdateEraAsync(era);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error updating the era: {error}", ex.Message);
            return StatusCode(500, "Error updating the era.");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult> DeleteEraAsync(int id)
    {
        try
        {
            await _eraData.DeleteEraAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting the era: {error}", ex.Message);
            return StatusCode(500, "Error deleting the era");
        }
    }
}
