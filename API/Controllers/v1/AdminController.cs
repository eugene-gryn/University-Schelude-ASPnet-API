using API.ModelsDtos.Exceptions;
using BLL.DTO.Models.ExceptionBase;
using BLL.DTO.Models.UserModels;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UserImageDto = API.ModelsDtos.UserDtos.UserImageDto;

namespace API.Controllers.v1;

[Route("api/v1/admin/")]
[ApiController]
[Authorize(Roles = "Administrator")]
public class AdminController : ControllerBase {
    private readonly UserService _userS;

    public AdminController(UserService userS) {
        _userS = userS;
    }

    /// <summary>
    ///     Gets all list of users
    /// </summary>
    /// <param name="offset">Offset in the id's on query</param>
    /// <param name="limit">Count of elems that need to be provided</param>
    /// <returns>List of user info without any relations</returns>
    /// <exception cref="ExceptionModelBase">401 Unauthorized (wrong-token) - If token parsing get wrong!</exception>
    [HttpGet("users")]
    public async Task<ActionResult<List<UserInfoDto>>> UserList(int offset = 0, int limit = 10) {
        try {
            return Ok(await _userS.GetList(User, offset, limit));
        }
        catch (ExceptionModelBase e) {
            return StatusCode(e.StatusCode, new Error(e));
        }
        catch (Exception e) {
            return StatusCode(500, "System get something wrong happens!" + e.Message);
        }
    }

    /// <summary>
    ///     Get user details by id
    /// </summary>
    /// <param name="id">User Id column value</param>
    /// <param name="dependencies">
    ///     params separated by ','
    ///     <c>"usersRoles"</c> - to connect user role in groups list
    ///     <c>"homework"</c> - homework tasks list of the user
    /// </param>
    /// <exception cref="ExceptionModelBase">401 Unauthorized (wrong-token) - If token parsing get wrong!</exception>
    /// <exception cref="ExceptionModelBase">403 Forbidden (invalid-operation-access-to-user) - If token parsing get wrong!</exception>
    /// <exception cref="ExceptionModelBase">404 NotFound (user-not-found) - If user id is not related to any entity</exception>
    [HttpGet("user")]
    public async Task<ActionResult<UserDto>> GetUserById(int? id, string? dependencies) {
        try {
            var dependenciesList = dependencies?.Split(',');
            return Ok(await _userS.GetById(User, id, dependenciesList));
        }
        catch (ExceptionModelBase e) {
            return StatusCode(e.StatusCode, new Error(e));
        }
        catch (Exception e) {
            return StatusCode(500, "System get something wrong happens!" + e.Message);
        }
    }

    /// <summary>
    ///     Sets image to user profile
    /// </summary>
    /// <param name="id">Id of the user or null if need to own user image set</param>
    /// <param name="image">Image form file</param>
    /// <returns>Text result of the image upload - Ok(Successful image update!) or Not Ok(Fail, wrong image model!)</returns>
    /// <exception cref="ExceptionModelBase">400 BadRequest (image-too-big) - If image bigger than 500Kb</exception>
    /// <exception cref="ExceptionModelBase">400 BadRequest (not-image-type) - If file provided not image</exception>
    /// <exception cref="ExceptionModelBase">401 Unauthorized (wrong-token) - If token parsing get wrong!</exception>
    /// <exception cref="ExceptionModelBase">
    ///     403 Forbidden (invalid-operation-access-to-user) - If default user role try to set
    ///     image another user
    /// </exception>
    /// <exception cref="ExceptionModelBase">404 NotFound (user-not-found) - If user id is not related to any entity</exception>
    [HttpPatch("user/icon-avatar")]
    public async Task<ActionResult<string>> SetUserAvatar([FromQuery] int? id, IFormFile image) {
        try {
            if (image.Length > 500_000)
                return BadRequest(new Error("image-too-big", "Image should be less than 500 KB size!"));
            if (!image.ContentType.StartsWith("image"))
                return BadRequest(new Error("not-image-type", "Upload file should be image!"));

            using var ms = new MemoryStream();
            await image.CopyToAsync(ms);

            var res = await _userS.UploadImage(User, new BLL.DTO.Models.UserModels.UserImageDto {
                Image = ms,
                ContentType = image.ContentType
            }, id);


            return Ok(res ? "Successful image update!" : "Fail, wrong image model!");
        }
        catch (ExceptionModelBase e) {
            return StatusCode(e.StatusCode, new Error(e));
        }
        catch (Exception e) {
            return StatusCode(500, "System get something wrong happens!" + e.Message);
        }
    }

    /// <summary>
    ///     Gets image from user profile
    /// </summary>
    /// <param name="id">Id of the user or null if need to own user image set</param>
    /// <param name="dataBase64">bool value that tell to provide base64 format browser or default class format</param>
    /// <returns>base64 format browser or default class format</returns>
    /// <exception cref="ExceptionModelBase">400 BadRequest (user-image-null) - If user does not have image in profile!</exception>
    /// <exception cref="ExceptionModelBase">401 Unauthorized (wrong-token) - If token parsing get wrong!</exception>
    /// <exception cref="ExceptionModelBase">
    ///     403 Forbidden (invalid-operation-access-to-user) - If default user role try to set
    ///     image another user
    /// </exception>
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
            return StatusCode(e.StatusCode, new Error(e));
        }
        catch (Exception e) {
            return StatusCode(500, "System get something wrong happens!" + e.Message);
        }
    }

    /// <summary>
    ///     Update information about user
    /// </summary>
    /// <param name="id">Empty if current user (Id for required user *Admins only)</param>
    /// <param name="update">Update class with special scheme</param>
    /// <returns>Updated user information</returns>
    /// <exception cref="ExceptionModelBase">401 Unauthorized (wrong-token) - If token parsing get wrong!</exception>
    /// <exception cref="ExceptionModelBase">
    ///     403 Forbidden (invalid-operation-access-to-user) - If user try to update info
    ///     about other user
    /// </exception>
    [HttpPut("users")]
    public async Task<ActionResult<UserDto>> UpdateById(int? id, UserUpdateDto update)
    {
        try
        {
            return Ok(await _userS.UpdateInfo(User, id, update));
        }
        catch (ExceptionModelBase e)
        {
            return StatusCode(e.StatusCode, new Error(e));
        }
        catch (Exception)
        {
            return StatusCode(500, "System get something wrong happens!");
        }
    }

    /// <summary>
    ///     Change property admin for other user
    /// </summary>
    /// <param name="id">Id int value for required user</param>
    /// <param name="adminValue">boolean value for change property (true if make admin, false if remove admin)</param>
    /// <returns>boolean result of the operation</returns>
    /// <exception cref="ExceptionModelBase">401 Unauthorized (wrong-token) - If token parsing get wrong!</exception>
    /// <exception cref="ExceptionModelBase">
    ///     403 Forbidden (invalid-operation-access-to-user) - If user try to change property
    ///     admin
    /// </exception>
    /// <exception cref="ExceptionModelBase">
    ///     403 Forbidden (restricted-admin-only-action) - This method is for admin purpose
    ///     only
    /// </exception>
    [HttpPatch("users/{id}&admin={adminValue}")]
    public async Task<ActionResult<bool>> UpdateAdmin(int id, bool adminValue)
    {
        try
        {
            return Ok(await _userS.ChangeAdminProperty(User, id, adminValue));
        }
        catch (ExceptionModelBase e)
        {
            return StatusCode(e.StatusCode, new Error(e));
        }
        catch (Exception)
        {
            return StatusCode(500, "System get something wrong happens!");
        }
    }

    /// <summary>
    ///     Function for change password for current user
    /// </summary>
    /// <param name="id">Required user (admin only!) or can be current id</param>
    /// <param name="renew">(Required!) New password</param>
    /// <returns>boolean result for action</returns>
    /// <exception cref="ExceptionModelBase">401 Unauthorized (wrong-token) - If token parsing get wrong!</exception>
    /// <exception cref="ExceptionModelBase">
    ///     403 Forbidden (invalid-operation-access-to-user) - If user try to change other
    ///     user password
    /// </exception>
    /// <exception cref="ExceptionModelBase">404 NotFound (user-not-found) - If user id is not related to any entity</exception>
    [HttpPatch("user/password")]
    public async Task<ActionResult<bool>> ChangePassword(int id, [Required] string renew)
    {
        try
        {
            return Ok(await _userS.ChangePassword(User, id, null, renew));
        }
        catch (ExceptionModelBase e)
        {
            return StatusCode(e.StatusCode, new Error(e));
        }
        catch (Exception)
        {
            return StatusCode(500, "System get something wrong happens!");
        }
    }

    /// <summary>
    ///     Delete current user profile
    /// </summary>
    /// <param name="id">OPTIONAL current user id or any profile(for admins only)</param>
    /// <returns>Result of the operation</returns>
    /// <exception cref="ExceptionModelBase">401 Unauthorized (wrong-token) - If token parsing get wrong!</exception>
    /// <exception cref="ExceptionModelBase">404 NotFound (user-not-found) - If user id is not related to any entity</exception>
    [HttpDelete("user")]
    public async Task<ActionResult<bool>> Delete(int? id)
    {
        try
        {
            return Ok(await _userS.Delete(User, id, null));
        }
        catch (ExceptionModelBase e)
        {
            return StatusCode(e.StatusCode, new Error(e));
        }
        catch (Exception)
        {
            return StatusCode(500, "System get something wrong happens!");
        }
    }
}