using System.ComponentModel.DataAnnotations;
using API.ModelsDtos.Exceptions;
using BLL.DTO.Models.ExceptionBase;
using BLL.DTO.Models.UserModels;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using BLL.DTO.Models.GroupsModels;
using UserImageDto = API.ModelsDtos.UserDtos.UserImageDto;
using static System.Net.Mime.MediaTypeNames;

namespace API.Controllers.v1;

/// <summary>
///     Class with User API function
/// </summary>
/// <exception cref="ExceptionModelBase">401 Unauthorized - if token not provided!</exception>
[Route("api/v1/")]
[ApiController]
[Authorize]
public class UserController : ControllerBase {
    private readonly UserService _userS;

    public UserController(UserService userS) {
        _userS = userS;
    }

    /// <summary>
    ///     Gets list of own user
    /// </summary>
    /// <param name="offset">Offset in the id's on query</param>
    /// <param name="limit">Count of elems that need to be provided</param>
    /// <returns>Logged user without any relations</returns>
    /// <exception cref="ExceptionModelBase">401 Unauthorized (wrong-token) - If token parsing get wrong!</exception>
    [HttpGet("current-user")]
    public async Task<ActionResult<UserInfoDto>> GetCurrentUser()
    {
        try
        {
            return Ok((await _userS.GetList(User)).First());
        }
        catch (ExceptionModelBase e)
        {
            return StatusCode(e.StatusCode, new Error(e));
        }
        catch (Exception e)
        {
            return StatusCode(500, "System get something wrong happens!" + e.Message);
        }
    }

    /// <summary>
    ///     Get user details with dependency
    /// </summary>
    /// <param name="dependencies">
    ///     params separated by ','
    ///     <c>"usersRoles"</c> - to connect user role in groups list
    ///     <c>"homework"</c> - homework tasks list of the user
    /// </param>
    /// <exception cref="ExceptionModelBase">401 Unauthorized (wrong-token) - If token parsing get wrong!</exception>
    /// <exception cref="ExceptionModelBase">403 Forbidden (invalid-operation-access-to-user) - If token parsing get wrong!</exception>
    [HttpGet("user")]
    public async Task<ActionResult<UserDto>> GetUserWithRelations(string? dependencies) {
        try
        {
            var dependenciesList = dependencies?.Split(',');
            return Ok(await _userS.GetById(User, null, dependenciesList));
        }
        catch (ExceptionModelBase e)
        {
            return StatusCode(e.StatusCode, new Error(e));
        }
        catch (Exception e)
        {
            return StatusCode(500, "System get something wrong happens!" + e.Message);
        }
    }

    /// <summary>
    ///     Sets image to user profile
    /// </summary>
    /// <param name="image">Image form file</param>
    /// <returns>Text result of the image upload - Ok(Successful image update!) or Not Ok(Fail, wrong image model!)</returns>
    /// <exception cref="ExceptionModelBase">400 BadRequest (image-too-big) - If image bigger than 500Kb</exception>
    /// <exception cref="ExceptionModelBase">400 BadRequest (not-image-type) - If file provided not image</exception>
    /// <exception cref="ExceptionModelBase">401 Unauthorized (wrong-token) - If token parsing get wrong!</exception>
    /// <exception cref="ExceptionModelBase">404 NotFound (user-not-found) - If user id is not related to any entity</exception>
    [HttpPatch("user/icon-avatar")]
    public async Task<ActionResult<string>> SetUserAvatar(IFormFile image)
    {
        try
        {
            if (image.Length > 500_000)
                return BadRequest(new Error("image-too-big", "Image should be less than 500 KB size!"));
            if (!image.ContentType.StartsWith("image"))
                return BadRequest(new Error("not-image-type", "Upload file should be image!"));

            using var ms = new MemoryStream();
            await image.CopyToAsync(ms);

            var res = await _userS.UploadImage(User, new BLL.DTO.Models.UserModels.UserImageDto
            {
                Image = ms,
                ContentType = image.ContentType
            }, null);


            return Ok(res ? "Successful image update!" : "Fail, wrong image model!");
        }
        catch (ExceptionModelBase e)
        {
            return StatusCode(e.StatusCode, new Error(e));
        }
        catch (Exception e)
        {
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
    public async Task<ActionResult<UserImageDto>> GetUserImage(bool dataBase64 = true)
    {
        try
        {
            var image = await _userS.GetImage(User, null);

            if (image == null) NotFound("User don't have profile avatar!");

            var model = new UserImageDto
            {
                ProfileImage = Convert.ToBase64String(image!.Image.ToArray()),
                ContentType = image.ContentType
            };

            if (dataBase64) return Ok($"data:{model.ContentType};base64,{model.ProfileImage}");

            return Ok(model);
        }
        catch (ExceptionModelBase e)
        {
            return StatusCode(e.StatusCode, new Error(e));
        }
        catch (Exception e)
        {
            return StatusCode(500, "System get something wrong happens!" + e.Message);
        }
    }

    /// <summary>
    ///     Gets boolean value of is login already used (for registration purposes)
    /// </summary>
    /// <param name="login">String login</param>
    /// <returns>boolean result</returns>
    [AllowAnonymous]
    [HttpGet("users/used-login={login}")]
    public async Task<ActionResult<bool>> IsLoginAlreadyUsed(string login)
    {
        try
        {
            return await _userS.IsLoginUsed(login);
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
    ///     Update information about user
    /// </summary>
    /// <param name="update">Update class with special scheme</param>
    /// <returns>Updated user information</returns>
    /// <exception cref="ExceptionModelBase">401 Unauthorized (wrong-token) - If token parsing get wrong!</exception>
    /// <exception cref="ExceptionModelBase">
    ///     403 Forbidden (invalid-operation-access-to-user) - If user try to update info
    ///     about other user
    /// </exception>
    [HttpPut("users")]
    public async Task<ActionResult<UserDto>> UpdateCurrentUser(UserUpdateDto update)
    {
        AdminController admin = new AdminController(_userS);

        return await admin.UpdateById(null, update);
    }


    /// <summary>
    ///     Function for change password for current user
    /// </summary>
    /// <param name="old">Old password that actually used to login (can be empty for admins(!))</param>
    /// <param name="renew">(Required!) New password</param>
    /// <returns>boolean result for action</returns>
    /// <exception cref="ExceptionModelBase">401 Unauthorized (wrong-token) - If token parsing get wrong!</exception>
    /// <exception cref="ExceptionModelBase">403 Forbidden (operation-password-needed) - If user does not provided any password</exception>
    /// <exception cref="ExceptionModelBase">400 BadRequest (password-compare-not-equal) - If old password is not correct</exception>
    [HttpPatch("user/password")]
    public async Task<ActionResult<bool>> ChangePassword([Required]string old, [Required] string renew)
    {
        try
        {
            return Ok(await _userS.ChangePassword(User, null, old, renew));
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
    /// <param name="password">Password to the account (empty for admins!)</param>
    /// <returns>Result of the operation</returns>
    /// <exception cref="ExceptionModelBase">401 Unauthorized (wrong-token) - If token parsing get wrong!</exception>
    /// <exception cref="ExceptionModelBase">400 BadRequest (password-compare-not-equal) - If password is not correct</exception>
    /// <exception cref="ExceptionModelBase">403 Forbidden (operation-password-needed) - If user does not provided any password</exception>
    /// <exception cref="ExceptionModelBase">404 NotFound (user-not-found) - If user is not related to any entity</exception>
    [HttpDelete("user")]
    public async Task<ActionResult<bool>> Delete(string password)
    {
        try
        {
            return Ok(await _userS.Delete(User, null, password));
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
    /// Gets request to respond with list user groups
    /// </summary>
    /// <returns>List of entities GroupGto</returns>
    /// <exception cref="ExceptionModelBase">401 Unauthorized (wrong-token) - If token parsing get wrong!</exception>
    /// <exception cref="ExceptionModelBase">403 Forbidden (operation-password-needed) - If user does not provided any password</exception>
    [HttpGet("user/groups")]
    public async Task<ActionResult<List<UserRoleDto>>> GetUserGroup() {
        try {
            return Ok(await _userS.GetUserGroups(User));
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
    /// Controller method that join user into group
    /// </summary>
    /// <param name="groupId">Group to join by ID</param>
    /// <returns>true - if operation successful, false if group is private or groupId is wrong</returns>
    /// <exception cref="ExceptionModelBase">400 BadRequest (wrong-count-user-groups) - If user attempt to broke limits(5) in group membership</exception>
    /// <exception cref="ExceptionModelBase">401 Unauthorized (wrong-token) - If token parsing get wrong!</exception>
    /// <exception cref="ExceptionModelBase">403 Forbidden (invalid-operation-access-to-user) - If token parsing get wrong!</exception>
    [HttpPut("/user/groups")]
    public async Task<ActionResult<bool>> JoinUserInGroup([Required] int groupId) {
        try
        {
            return Ok(await _userS.JoinUserInGroup(User, groupId));
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
    /// Controller method that makes it possible to leave from group
    /// </summary>
    /// <param name="groupId">Group to leave from Id</param>
    /// <returns>Result, can be true if operation successful or false of groupId is wrong or user not consist in this group</returns>
    /// <exception cref="ExceptionModelBase">401 Unauthorized (wrong-token) - If token parsing get wrong!</exception>
    /// <exception cref="ExceptionModelBase">403 Forbidden (invalid-operation-access-to-user) - If token parsing get wrong!</exception>

    [HttpDelete("/user/groups")]
    public async Task<ActionResult<bool>> LeaveUserFromGroup([Required] int groupId) {
        try
        {
            return Ok(await _userS.LeaveUserFromGroup(User, groupId));
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