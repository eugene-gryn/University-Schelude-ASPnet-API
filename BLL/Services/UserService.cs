using System.Net;
using System.Security.Claims;
using AutoMapper;
using BLL.DTO.Models.ExceptionBase;
using BLL.DTO.Models.JWTManager;
using BLL.DTO.Models.UserModels;
using BLL.DTO.Models.UserModels.Password;
using BLL.Services.RolesHandler;
using DAL.Entities;
using DAL.UOW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BLL.Services;

public class UserService : BaseService {
    private IConfiguration _conf;

    public UserService(IRoleHandler roles, IUnitOfWork uow, IMapper mapper, IJwtManagerRepository jwt,
        IConfiguration conf) : base(uow,
        mapper, jwt, roles) {
        _conf = conf;
    }

    public async Task<KeyValuePair<string, TokensDto>> Login(string username, string password) {
        var tokens = await JwtManager.RegisterToken(new UserLoginDto {
            Login = username,
            Password = password
        });

        if (tokens.Value == null)
            throw new ExceptionModelBase((int)HttpStatusCode.BadRequest, "Wrong login or password", "User",
                "Login tokenUser token");

        var user = await Uow.Users.Read().Where(u => u.Login == username)
            .Include(u => u.Token)
            .SingleOrDefaultAsync();

        user!.Token = Mapper.Map<Tokens>(tokens.Value);
        var res = await Uow.Users.UpdateAsync(user);
        if (res) Uow.Save();

        return tokens;
    }

    public async Task<UserRegisterDto?> Register(UserRegisterDto user) {
        var res = await Uow.Users.Add(Mapper.Map<User>(user));

        if (res) Uow.Save();

        if (!res)
            throw new ExceptionModelBase((int)HttpStatusCode.BadRequest, "Login already used", "User",
                "Register new tokenUser");

        return user;
    }

    public async Task<string> TokenUpdate(ClaimsPrincipal tokenUser, string refreshToken) {
        var id = await JwtManager.GetUserId(tokenUser);

        return await JwtManager.RefreshToken(id, refreshToken);
    }

    public async Task<KeyValuePair<string, TokensDto>> TokenReset(ClaimsPrincipal tokenUser, string refreshToken) {
        var id = await JwtManager.GetUserId(tokenUser);

        return await JwtManager.ResetToken(id, refreshToken);
    }

    public async Task<List<UserInfoDto>> GetList(ClaimsPrincipal user, int offset = 0, int limit = 10) {
        var role = await Roles.GetUserRole(user);
        List<UserInfoDto> list;

        if (role == UserRoles.Administrator)
            list = Uow.Users.Read().Where(user1 => user1.Id > offset).Take(limit).AsEnumerable()
                .Select(u => Mapper.Map<UserInfoDto>(u)).ToList();
        else
            list = Uow.Users.ReadById(await JwtManager.GetUserId(user)).AsEnumerable()
                .Select(u => Mapper.Map<UserInfoDto>(u)).ToList();

        return list;
    }

    public async Task<bool> UploadImage(ClaimsPrincipal user, UserImageDto image, int? id) {
        var role = await Roles.GetUserRole(user);
        var userNotFound = new ExceptionModelBase((int)HttpStatusCode.NotFound, "User with this Id not found",
            "User Image", "Upload Image");
        var idToGetUser = role == UserRoles.Administrator && id != null ? id.Value : Roles.UserId!.Value;


        if (role == UserRoles.User && id != null && Roles.UserId != id)
            throw new ExceptionModelBase((int)HttpStatusCode.Forbidden,
                "You try to edit other user image", "User image", "UpdateAsync image");

        var userEntity = await Uow.Users.ReadById(idToGetUser)
            .Include(u => u.ProfileImage)
            .SingleOrDefaultAsync() ?? throw userNotFound;

        userEntity.ProfileImage = Mapper.Map<UserImage>(image);
        var res = await Uow.Users.UpdateAsync(userEntity);
        if (res) Uow.Save();

        return res;
    }

    public async Task<UserImageDto?> GetImage(ClaimsPrincipal user, int? id) {
        var role = await Roles.GetUserRole(user);
        var userNotFound =
            new ExceptionModelBase((int)HttpStatusCode.BadRequest,
                "User don't have profile image. Please provide image!", "User Image", "Get Image");
        UserImageDto? userImage = null;
        var idToGetUser = role == UserRoles.Administrator && id != null ? id.Value : Roles.UserId!.Value;

        if (role == UserRoles.User && id != null && Roles.UserId != id)
            throw new ExceptionModelBase((int)HttpStatusCode.Forbidden,
                "You try to edit other user image", "User image", "UpdateAsync image");

        userImage = Mapper.Map<UserImageDto>((await Uow.Users.ReadById(idToGetUser)
            .Include(u => u.ProfileImage)
            .SingleOrDefaultAsync())?.ProfileImage ?? throw userNotFound);

        return userImage;
    }

    public async Task<UserDto?> GetById(ClaimsPrincipal user, int? id, string[]? dependencies) {
        var userNotFound = new ExceptionModelBase((int)HttpStatusCode.NotFound, "User with this Id not found", "User",
            "Get user info");
        var role = await Roles.GetUserRole(user);

        UserDto? received = null;
        var idToGetUser = role == UserRoles.Administrator && id != null ? id.Value : Roles.UserId!.Value;

        if (role == UserRoles.User && id != null && Roles.UserId != id)
            throw new ExceptionModelBase((int)HttpStatusCode.Forbidden,
                "You trying to get info about other user!", "User info", "Get user info");

        var q = Uow.Users.ReadById(idToGetUser);

        if (dependencies != null) {
            if (dependencies.Contains("usersRoles")) q = q.Include(u => u.UsersRoles);
            if (dependencies.Contains("homework")) q = q.Include(u => u.Homework);
        }

        received = Mapper.Map<UserDto>(await q.SingleOrDefaultAsync() ?? throw userNotFound);

        return received;
    }

    public async Task<UserDto?> UpdateInfo(ClaimsPrincipal user, int? id, UserUpdateDto updateDto) {
        var role = await Roles.GetUserRole(user);
        if (role == UserRoles.User && id != null && Roles.UserId != id)
            throw new ExceptionModelBase((int)HttpStatusCode.Forbidden,
                "You trying to get info about other user!", "User update", "Update user info");

        var idToGetUser = role == UserRoles.Administrator && id != null ? id.Value : Roles.UserId!.Value;

        var userUpdate = await Uow.Users.ReadById(idToGetUser).SingleOrDefaultAsync();

        if (userUpdate == null)
            throw new ExceptionModelBase((int)HttpStatusCode.NotFound, "User with this Id not found", "User",
                "Update user info");


        if (!string.IsNullOrEmpty(updateDto.Name)) userUpdate.Name = updateDto.Name;
        if (!string.IsNullOrEmpty(updateDto.TelegramToken)) userUpdate.TelegramToken = updateDto.TelegramToken;
        if (updateDto.Settings != null) userUpdate.Settings = Mapper.Map<Settings>(updateDto.Settings);

        var res = await Uow.Users.UpdateAsync(userUpdate);
        if (res) Uow.Save();

        return Mapper.Map<UserDto>(userUpdate);
    }

    public async Task<bool> IsLoginUsed(string login) {
        return await Uow.Users.Read().AnyAsync(u => u.Login == login);
    }

    // Test
    public async Task<bool> ChangeAdminProperty(ClaimsPrincipal user, int id, bool adminValue) {
        var role = await Roles.GetUserRole(user);

        if (role == UserRoles.Administrator) {
            var userUpdate = await Uow.Users.ReadById(id).SingleOrDefaultAsync();
            if (userUpdate == null)
                throw new ExceptionModelBase((int)HttpStatusCode.NotFound, "User with this Id not found", "User",
                    "Make user admin");

            userUpdate.IsAdmin = adminValue;

            var res = await Uow.Users.UpdateAsync(userUpdate);
            if (res) Uow.Save();

            return res;
        }
        
        throw new ExceptionModelBase((int)HttpStatusCode.Forbidden,
                "You trying to make someone admin!", "User admin", "Make user admin!");
    }

    // Test
    public async Task<KeyValuePair<string, TokensDto>> ChangePassword(ClaimsPrincipal user, int? id, string? old,
        string renew) {
        var role = await Roles.GetUserRole(user);
        if (role == UserRoles.User && id != null && Roles.UserId != id) throw new ExceptionModelBase((int)HttpStatusCode.Forbidden,
                "You trying to get info about other user!", "User password", "Change user password");

        var idToGetUser = role == UserRoles.Administrator && id != null ? id.Value : Roles.UserId!.Value;

        if (old == null && role == UserRoles.User) throw new ExceptionModelBase((int)HttpStatusCode.Forbidden,
            "You need to provide old password for the change!", "User password", "Change user password");

        var userPassword = await Uow.Users.ReadById(idToGetUser).Include(u => u.Token).SingleOrDefaultAsync() 
                           ?? throw new ExceptionModelBase((int)HttpStatusCode.NotFound, "User with this Id not found", "User", "Change user password");

        if (role == UserRoles.User && !PasswordSingleton.Password.VerifyPassword(old!, userPassword.Password, userPassword.Salt))
            throw new ExceptionModelBase((int)HttpStatusCode.BadRequest, "Old password is not equal!", "User password", "Change user password");

        PasswordSingleton.Password.CreatePasswordHash(renew, out byte[] newHash, out byte[] newSalt);

        userPassword.Password = newHash;
        userPassword.Salt = newSalt;

        var tokens = await JwtManager.ResetToken(idToGetUser, userPassword.Token.RefreshToken);

        userPassword.Token = Mapper.Map<Tokens>(tokens.Value);

        var res = await Uow.Users.UpdateAsync(userPassword);
        if (res) Uow.Save();

        return tokens;
    }

    // Test
    public async Task<bool> Delete(ClaimsPrincipal user, int? id, string? password) {
        var role = await Roles.GetUserRole(user);
        if (role == UserRoles.User && id != null && Roles.UserId != id) throw new ExceptionModelBase((int)HttpStatusCode.Forbidden,
            "You trying delete other user!", "User", "Remove user");

        var idToGetUser = role == UserRoles.Administrator && id != null ? id.Value : Roles.UserId!.Value;

        if (password == null && role == UserRoles.User) throw new ExceptionModelBase((int)HttpStatusCode.Forbidden,
            "You need to provide password to remove your account", "User", "Remove user");

        var userPassword = await Uow.Users.ReadById(idToGetUser).Include(u => u.Token).SingleOrDefaultAsync()
                           ?? throw new ExceptionModelBase((int)HttpStatusCode.NotFound, "User with this Id not found", "User", "Remove user");

        if (role == UserRoles.User && !PasswordSingleton.Password.VerifyPassword(password!, userPassword.Password, userPassword.Salt))
            throw new ExceptionModelBase((int)HttpStatusCode.BadRequest, "Password is not equal!", "User", "Remove user");

        var res = await Uow.Users.Delete(idToGetUser);
        if (res) Uow.Save();

        return res;
    }
}