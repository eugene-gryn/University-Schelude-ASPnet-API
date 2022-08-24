using BLL.DTO.Models.UserModels;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.v1;

[Route("api/v1/")]
[ApiController]
public class AuthorizationController : ControllerBase {
    private readonly UserService _userS;

    public AuthorizationController(UserService userS) {
        _userS = userS;
    }

    [HttpGet("authorize")]
    public async Task<ActionResult<TokensDTO>> Authorize(string login, string password) {

        try {
            var token = await _userS.Login(login, password);

            if (token == null) return Unauthorized("Invalid login or password!");

            return Ok(token);
        }
        catch (Exception) {
            return StatusCode(500, "Something wrong happens!");
        }
    }
}