using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Server.Library.Models;

namespace VintageHub.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes")]
public class UserController : ControllerBase
{
    private readonly IUserData _userData;
    private readonly ILogger<UserController> _logger;

    public UserController(
        IUserData userData,
        ILogger<UserController> logger)
    {
        _userData = userData;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserModel>>> GetAllUsersAsync()
    {
        try
        {
            var users = await _userData.GetAllUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching the users: {error}", ex.Message);
            return StatusCode(500, "Error fetching the users.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserModel>> GetUserByIdAsync(int id)
    {
        try
        {
            var user = await _userData.GetUserByIdAsync(id);

            if (user is null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }
        catch (Exception ex) 
        {
            _logger.LogError("Error fetching user by id: {error}", ex.Message);
            return StatusCode(500, $"Error fetching the user by id of {id}");
        }
    }

    [HttpGet("auth/{oid}")]
    public async Task<ActionResult<UserModel>> GetUserByOidAsync(string oid)
    {
        try
        {
            var user = await _userData.GetUserByOidAsync(oid);

            if (user is null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching user by OID: {error}", ex.Message);
            return StatusCode(500, $"Error fetching user by OID of {oid}");
        }
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<UserModel>> InsertUserAsync([FromBody] UserModel user)
    {
        if (ModelState.IsValid is false)
        {
            return BadRequest(ModelState);
        }

        try
        {
            int createdUserId = await _userData.InsertUserAsync(user);

            return CreatedAtAction(nameof(GetUserByIdAsync), new { id = createdUserId }, user);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error inserting user: {error}", ex.Message);
            return StatusCode(500, $"Error inserting the user.");
        }
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUserAsync([FromBody] UserModel user)
    {
        if (ModelState.IsValid is false)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _userData.UpdateUserAsync(user);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error updating user: {error}", ex.Message);
            return StatusCode(500, $"Error updating the user.");
        }
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteUserAsync([FromBody] UserModel user)
    {
        if (ModelState.IsValid is false)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _userData.DeleteUserAsync(user);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting user: {error}", ex.Message);
            return StatusCode(500, $"Error deleting user.");
        }
    }
}
