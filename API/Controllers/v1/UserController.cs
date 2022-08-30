using System.Net;
using BLL.DTO.Models.ExceptionBase;
using BLL.DTO.Models.UserModels;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.v1;

[Route("api/v1/")]
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

    [HttpPost("users/icon-avatar")]
    public async Task<ActionResult<string>> SetUserAvatar([FromQuery]int? id, IFormFile image) {
        try {
            if (image.Length > 500_000) return BadRequest("Image should be less than 500 KB size");
            if (!image.ContentType.StartsWith("image")) return BadRequest("Upload file should be image!");

            using var ms = new MemoryStream();
            await image.CopyToAsync(ms);

            var res = await _userS.UploadImage(User, new UserImageDto() {
                ProfileImage = ms,
                ContentType = image.ContentType
            }, id);


            return Ok(res ? "Successful image update!" : "Fail, wrong image model!");
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

    [HttpGet("users/icon-avatar")]
    public async Task<ActionResult<ModelsDtos.UserDtos.UserImageDto>> GetUserImage([FromQuery]int? id, bool dataBase64 = true) {
        try {
            var image = await _userS.GetImage(User, id);

            if (image == null) NotFound("User don't have profile avatar!");

            var model = new ModelsDtos.UserDtos.UserImageDto() {
                ProfileImage = Convert.ToBase64String(image.ProfileImage.ToArray()),
                ContentType = image.ContentType
            };

            if (dataBase64) {
                return Ok($"data:{model.ContentType};base64,{model.ProfileImage}");
            }

            return Ok(model);
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