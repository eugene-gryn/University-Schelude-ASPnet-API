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

            if (token == null) return Unauthorized("Invalid login or password!");

            return Ok(token);
        }
        catch (Exception) {
            return StatusCode(500, "Something wrong happens!");
        }
    }

    [AllowAnonymous]
    [HttpPost("authorize")]
    public async Task<ActionResult<UserRegisterDto>> Register(UserRegisterDto user) {
        try
        {
            var res = await _userS.Register(user);

            if (res == null) BadRequest("Something wrong happens!");

            return Ok(res);
        }
        catch (Exception)
        {
            return StatusCode(500, "Something wrong happens!");
        }

    }



}