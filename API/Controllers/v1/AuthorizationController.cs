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

    [AllowAnonymous]
    [HttpGet("authorize")]
    public async Task<ActionResult<TokensDto>> Login(string login, string password) {
    
        try {
            var token = await _userS.Login(login, password);
    
            return Ok(token);
        }
        catch (ExceptionModelBase e)
        {
            return StatusCode(e.StatusCode, $"{e.Message} - Model: {e.ModelName} | Action: {e.ActionName}");
        }
        catch (Exception)
        {
            return StatusCode(500, "System get something wrong happens!");
        }
    }

    [AllowAnonymous]
    [HttpPost("authorize")]
    public async Task<ActionResult<UserRegisterDto>> Register(UserRegisterDto user) {
        try
        {
            var res = await _userS.Register(user);
    
            return Ok(res);
        }
        catch (ExceptionModelBase e)
        {
            return StatusCode(e.StatusCode, $"{e.Message} - Model: {e.ModelName} | Action: {e.ActionName}");
        }
        catch (Exception)
        {
            return StatusCode(500, "System get something wrong happens!");
        }
    }
    
    [HttpPost("refresh-token")]
    public async Task<ActionResult<TokensDto>> RefreshToken(string refreshToken) {
        try {
            var tokens = await _userS.TokenUpdate(User, refreshToken);

            return Ok(tokens);
        }
        catch (ExceptionModelBase e)
        {
            return StatusCode(e.StatusCode, $"{e.Message} - Model: {e.ModelName} | Action: {e.ActionName}");
        }
        catch (Exception)
        {
            return StatusCode(500, "System get something wrong happens!");
        }
    }
}