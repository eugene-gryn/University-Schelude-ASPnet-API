using BLL.DTO.Models.UserModels;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.v1;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase {
    //[HttpGet("users")]
    //public async Task<ActionResult<UserRegisterDto>> UserList(int offset, int limit)
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
    //
    //[HttpGet("users")]
    //public async Task<ActionResult<UserRegisterDto>> UserById(int id)
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