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
            var createdUser = await _userData.GetUserByIdAsync(createdUserId);

            return Ok(createdUser);
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

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUserAsync(int id)
    {
        try
        {
            await _userData.DeleteUserAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting user: {error}", ex.Message);
            return StatusCode(500, $"Error deleting user.");
        }
    }
}
