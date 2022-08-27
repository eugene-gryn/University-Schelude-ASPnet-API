using BLL.DTO.Models.ExceptionBase;
using BLL.DTO.Models.UserModels;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.v1;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase {

    private readonly UserService _userS;

    public UserController(UserService userS)
    {
        _userS = userS;
    }

    [HttpGet("users")]
    public async Task<ActionResult<List<UserInfoDto>>> UserList(int offset = 0, int limit = 10)
    {
        try {
            return Ok(await _userS.GetUsers(User, offset, limit));
        }
        catch (ExceptionModelBase e)
        {
            return StatusCode(e.StatusCode, $"{e.Message} - Model: {e.ModelName} | Action: {e.ActionName}");
        }
        catch (Exception e)
        {
            return StatusCode(500, "System get something wrong happens!" + e.Message);
        }
    }
    //[HttpGet("users")]
    //public async Task<ActionResult<UserRegisterDto>> UserById(int id)
    //{
    //    try
    //    {
    //        // TODO: Code here....
    //        // TODO: DTO FULL INFO USER
    //    }
    //    catch (ExceptionModelBase e)
    //    {
    //        return BadRequest($"{e.Message} - Model: {e.ModelName} | Action: {e.ActionName}");
    //    }
    //    catch (Exception)
    //    {
    //        return StatusCode(500, "System get something wrong happens!");
    //    }
    //}
    //
    //[HttpPut("users")]
    //public async Task<ActionResult<UserRegisterDto>> UpdateUserById(int id)
    //{
    //    try
    //    {
    //        // TODO: Code here....
    //    }
    //    catch (ExceptionModelBase e)
    //    {
    //        return BadRequest($"{e.Message} - Model: {e.ModelName} | Action: {e.ActionName}");
    //    }
    //    catch (Exception)
    //    {
    //        return StatusCode(500, "System get something wrong happens!");
    //    }
    //}
}