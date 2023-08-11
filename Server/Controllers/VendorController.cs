using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Server.Library.Models;

namespace VintageHub.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes")]
public class VendorController : ControllerBase
{
    private readonly IVendorData _vendorData;
    private readonly ILogger<VendorController> _logger;

    public VendorController(
        IVendorData vendorData,
        ILogger<VendorController> logger)
    {
        _vendorData = vendorData;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<VendorModel>>> GetAllVendorsAsync()
    {
        try
        {
            var vendors = await _vendorData.GetAllVendorsAsync();
            return Ok(vendors);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching the vendors: {error}", ex.Message);
            return StatusCode(500, "Error fetching the vendors");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VendorModel>> GetVendorByIdAsync(int id)
    {
        try
        {
            var vendor = await _vendorData.GetVendorByIdAsync(id);

            if (vendor is null)
            {
                return NotFound("Vendor not found.");
            }

            return Ok(vendor);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching the vendor by id: {error}", ex.Message);
            return StatusCode(500, $"Error fetching vendor by if of {id}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<VendorModel>> InsertVendorAsync([FromBody] VendorModel vendor)
    {
        if (ModelState.IsValid is false)
        {
            return BadRequest(ModelState);
        }

        try
        {
            int createdVendorId = await _vendorData.InsertVendorAsync(vendor);

            return CreatedAtAction(nameof(GetVendorByIdAsync), new { id = createdVendorId }, vendor);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error inserting the vendor: {error}", ex.Message);
            return StatusCode(500, "Error inserting the vendor.");
        }
    }

    [HttpPut]
    public async Task<ActionResult> UpdateVendorAsync([FromBody] VendorModel vendor)
    {
        if (ModelState.IsValid is false)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _vendorData.UpdateVendorAsync(vendor);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error updating the vendor: {error}", ex.Message);
            return StatusCode(500, "Error updating the vendor");
        }
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteVendorAsync([FromBody] VendorModel vendor)
    {
        if (ModelState.IsValid is false)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _vendorData.DeleteVendorAsync(vendor);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting the vendor: {error}", ex.Message);
            return StatusCode(500, "Error deleting the vendor.");
        }
    }
}
