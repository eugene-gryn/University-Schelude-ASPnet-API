using API.ModelsDtos.Exceptions;
using BLL.DTO.Models.ExceptionBase;
using BLL.DTO.Models.UserModels;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.v1;

[Route("api/v1/")]
[ApiController]
[Authorize]
public class AuthorizationController : ControllerBase {
    private readonly UserService _userS;

    public AuthorizationController(UserService userS) {
        _userS = userS;
    }

    /// <summary>
    /// Takes refresh token from DB and generate token for access
    /// </summary>
    /// <param name="login">Not starts with number</param>
    /// <param name="password">any letter and digit</param>
    /// <returns>KeyValue pair - Key: token, Value: refresh token</returns>
    /// <exception cref="ExceptionModelBase">403 Forbidden (wrong-credentials-login) - If wrong login or password</exception>
    [HttpGet("token")]
    [AllowAnonymous]
    public async Task<ActionResult<KeyValuePair<string, TokensDto>>> Login(string login, string password) {
        try {
            var token = await _userS.Login(login, password);

            return Ok(token);
        }
        catch (ExceptionModelBase e) {
            return StatusCode(e.StatusCode, new Error(e));
        }
        catch (Exception e) {
            return StatusCode(500, $"System get something wrong happens! {e.Message}");
        }
    }

    /// <summary>
    /// Adds new user to DB
    /// </summary>
    /// <param name="user">
    /// User register DTO with validations,
    /// Login - not starts with digit
    /// Name - not starts with digit
    /// Password - must have at least one digit and letter
    /// TelegramToken - only digit string
    /// </param>
    /// <returns>Registered Dto</returns>
    [AllowAnonymous]
    [HttpPost("token")]
    public async Task<ActionResult<UserRegisterDto>> Register([FromBody]UserRegisterDto user) {
        try {
            var res = await _userS.Register(user);

            return Ok(res);
        }
        catch (ExceptionModelBase e) {
            return StatusCode(e.StatusCode, new Error(e));
        }
        catch (Exception e) {
            return StatusCode(500, $"System get something wrong happens! {e.Message}");
        }
    }

    /// <summary>
    /// Gets new token with refresh token
    /// </summary>
    /// <param name="refreshToken">Refresh token value that contains in DB</param>
    /// <returns>New Token</returns>
    /// <exception cref="ExceptionModelBase">401 Unauthorized (wrong-refresh-token) - If wrong refresh token provided</exception>
    [HttpPost("refresh-token")]
    public async Task<ActionResult<string>> RefreshToken([FromBody] string refreshToken) {
        try {
            var token = await _userS.TokenUpdate(User, refreshToken);

            return Ok(token);
        }
        catch (ExceptionModelBase e) {
            return StatusCode(e.StatusCode, new Error(e));
        }
        catch (Exception e) {
            return StatusCode(500, $"System get something wrong happens! {e.Message}");
        }
    }

    /// <summary>
    /// Reset refresh token and older tokens should be wrong
    /// </summary>
    /// <param name="refreshToken">Refresh token value that contains in DB</param>
    /// <returns>New Token Key: Token, Value: Refresh token </returns>
    /// <exception cref="ExceptionModelBase">401 Unauthorized (wrong-refresh-token) - If wrong refresh token provided</exception>
    /// <exception cref="ExceptionModelBase">404 Not Found (wrong-user-id) - If wrong user id provided!</exception>
    [HttpPost("reset-token")]
    public async Task<ActionResult<KeyValuePair<string, TokensDto>>> ResetToken([FromBody] string refreshToken) {
        try {
            var token = await _userS.TokenReset(User, refreshToken);

            return Ok(token);
        }
        catch (ExceptionModelBase e) {
            return StatusCode(e.StatusCode, new Error(e));
        }
        catch (Exception e) {
            return StatusCode(500, $"System get something wrong happens! {e.Message}");
        }
    }
}