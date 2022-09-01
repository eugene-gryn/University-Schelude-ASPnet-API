using BLL.DTO.Models.ExceptionBase;
using BLL.DTO.Models.UserModels;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserImageDto = API.ModelsDtos.UserDtos.UserImageDto;

namespace API.Controllers.v1;

[Route("api/v1/")]
[ApiController]
[Authorize]
public class UserController : ControllerBase {
    private readonly UserService _userS;

    public UserController(UserService userS) {
        _userS = userS;
    }

    [HttpGet("users")]
    public async Task<ActionResult<List<UserInfoDto>>> UserList(int offset = 0, int limit = 10) {
        try {
            return Ok(await _userS.GetList(User, offset, limit));
        }
        catch (ExceptionModelBase e) {
            return StatusCode(e.StatusCode, $"{e.Message} - Model: {e.ModelName} | Action: {e.ActionName}");
        }
        catch (Exception e) {
            return StatusCode(500, "System get something wrong happens!" + e.Message);
        }
    }

    [HttpGet("user")]
    public async Task<ActionResult<UserDto>> GetUserById(int? id, string? dependencies) {
        try {
            string[]? dependenciesList = dependencies?.Split(',');
            return Ok(await _userS.GetById(User, id, dependenciesList));
        }
        catch (ExceptionModelBase e) {
            return StatusCode(e.StatusCode, $"{e.Message} - Model: {e.ModelName} | Action: {e.ActionName}");
        }
        catch (Exception e) {
            return StatusCode(500, "System get something wrong happens!" + e.Message);
        }
    }

    [HttpPatch("user/icon-avatar")]
    public async Task<ActionResult<string>> SetUserAvatar([FromQuery] int? id, IFormFile image) {
        try {
            if (image.Length > 500_000) return BadRequest("Image should be less than 500 KB size");
            if (!image.ContentType.StartsWith("image")) return BadRequest("Upload file should be image!");

            using var ms = new MemoryStream();
            await image.CopyToAsync(ms);

            var res = await _userS.UploadImage(User, new BLL.DTO.Models.UserModels.UserImageDto {
                Image = ms,
                ContentType = image.ContentType
            }, id);


            return Ok(res ? "Successful image update!" : "Fail, wrong image model!");
        }
        catch (ExceptionModelBase e) {
            return StatusCode(e.StatusCode, $"{e.Message} - Model: {e.ModelName} | Action: {e.ActionName}");
        }
        catch (Exception e) {
            return StatusCode(500, "System get something wrong happens!" + e.Message);
        }
    }

    [HttpGet("user/icon-avatar")]
    public async Task<ActionResult<UserImageDto>> GetUserImage([FromQuery] int? id, bool dataBase64 = true) {
        try {
            var image = await _userS.GetImage(User, id);

            if (image == null) NotFound("User don't have profile avatar!");

            var model = new UserImageDto {
                ProfileImage = Convert.ToBase64String(image!.Image.ToArray()),
                ContentType = image.ContentType
            };

            if (dataBase64) return Ok($"data:{model.ContentType};base64,{model.ProfileImage}");

            return Ok(model);
        }
        catch (ExceptionModelBase e) {
            return StatusCode(e.StatusCode, $"{e.Message} - Model: {e.ModelName} | Action: {e.ActionName}");
        }
        catch (Exception e) {
            return StatusCode(500, "System get something wrong happens!" + e.Message);
        }
    }
    [AllowAnonymous]
    [HttpGet("users/used-login={login}")]
    public async Task<ActionResult<bool>> IsLoginAlreadyUsed(string login)
    {
        try {
            return await _userS.IsLoginUsed(login);
        }
        catch (ExceptionModelBase e)
        {
            return BadRequest($"{e.Message} - Model: {e.ModelName} | Action: {e.ActionName}");
        }
        catch (Exception)
        {
            return StatusCode(500, "System get something wrong happens!");
        }
    }
    
    [HttpPut("users")]
    public async Task<ActionResult<UserRegisterDto>> UpdateById(int? id, UserUpdateDto update)
    {
        try {
            return Ok(await _userS.UpdateInfo(User, id, update));
        }
        catch (ExceptionModelBase e)
        {
            return BadRequest($"{e.Message} - Model: {e.ModelName} | Action: {e.ActionName}");
        }
        catch (Exception)
        {
            return StatusCode(500, "System get something wrong happens!");
        }
    }
    [HttpPatch("users/{id}&admin={adminValue}")]
    public async Task<ActionResult<bool>> UpdateAdmin(int id, bool adminValue)
    {
        try {
            return Ok(await _userS.ChangeAdminProperty(User, id, adminValue));
        }
        catch (ExceptionModelBase e)
        {
            return BadRequest($"{e.Message} - Model: {e.ModelName} | Action: {e.ActionName}");
        }
        catch (Exception)
        {
            return StatusCode(500, "System get something wrong happens!");
        }
    }
    [HttpPatch("user/password")]
    public async Task<ActionResult<bool>> ChangePassword(int? id, string? old, string renew)
    {
        try {
            return Ok(await _userS.ChangePassword(User, id, old, renew));
        }
        catch (ExceptionModelBase e)
        {
            return BadRequest($"{e.Message} - Model: {e.ModelName} | Action: {e.ActionName}");
        }
        catch (Exception)
        {
            return StatusCode(500, "System get something wrong happens!");
        }
    }
    [HttpDelete("user")]
    public async Task<ActionResult<bool>> Delete(int? id, string? password)
    {
        try {
            return Ok(await _userS.Delete(User, id, password));
        }
        catch (ExceptionModelBase e)
        {
            return BadRequest($"{e.Message} - Model: {e.ModelName} | Action: {e.ActionName}");
        }
        catch (Exception)
        {
            return StatusCode(500, "System get something wrong happens!");
        }
    }
}