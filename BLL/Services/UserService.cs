using System.Net;
using System.Security.Claims;
using AutoMapper;
using BLL.DTO.Models.ExceptionBase;
using BLL.DTO.Models.JWTManager;
using BLL.DTO.Models.UserModels;
using BLL.Services.RolesHandler;
using DAL.Entities;
using DAL.UOW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BLL.Services;

public class UserService : BaseService {
    private IConfiguration _conf;

    public UserService(IRoleHandler roles, IUnitOfWork uow, IMapper mapper, IJwtManagerRepository jwt, IConfiguration conf) : base(uow,
        mapper, jwt, roles) {
        _conf = conf;
    }

    public async Task<KeyValuePair<string, TokensDto>> Login(string username, string password) {
        var tokens = await JwtManager.RegisterToken(new UserLoginDto {
            Login = username,
            Password = password
        });

        if (tokens.Value == null)
            throw new ExceptionModelBase((int)HttpStatusCode.BadRequest, "Wrong login or password", "User", "Login tokenUser token");

        var user = await Uow.Users.Read().Where(u => u.Login == username)
            .Include(u => u.Token)
            .SingleOrDefaultAsync();

        user!.Token = Mapper.Map<Tokens>(tokens.Value);
        var res = await Uow.Users.Update(user);
        if (res) Uow.Save();

        return tokens;
    }

    public async Task<UserRegisterDto?> Register(UserRegisterDto user) {
        var res = await Uow.Users.Add(Mapper.Map<User>(user));

        if (res) Uow.Save();

        if (!res) throw new ExceptionModelBase((int)HttpStatusCode.BadRequest, "Login already used", "User", "Register new tokenUser");

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

    public async Task<List<UserInfoDto>> GetUsers(ClaimsPrincipal user, int offset = 0, int limit = 10) {
        var role = await Roles.GetUserRole(user);
        List<UserInfoDto> list;

        if (role == UserRoles.Administrator)
            list = Uow.Users.Read().Where((user1) => user1.Id > offset).Take(limit).AsEnumerable()
                .Select(u => Mapper.Map<UserInfoDto>(u)).ToList();
        else list = Uow.Users.ReadById(await JwtManager.GetUserId(user)).AsEnumerable()
            .Select(u => Mapper.Map<UserInfoDto>(u)).ToList();

        return list;
    }

    public async Task<bool> UploadImage(ClaimsPrincipal user, UserImageDto image, int? id) {
        var role = await Roles.GetUserRole(user);
        var userNotFound = new ExceptionModelBase((int)HttpStatusCode.BadRequest, "User with this Id not found", "User Image", "Upload Image");
        User userEntity = new();

        if (role == UserRoles.User && id != null && Roles.UserId != id)
            throw new ExceptionModelBase((int)HttpStatusCode.Forbidden,
                "You try to edit other user image", "User image", "Update image");

        if (role == UserRoles.Administrator) {
            if (id != null)
                userEntity = await Uow.Users.ReadById(id.Value)
                    .Include(u => u.ProfileImage)
                    .SingleOrDefaultAsync() ?? throw userNotFound;
        }



        if(role == UserRoles.User || id == null) {
            if (Roles.UserId != null)
                userEntity = await Uow.Users.ReadById(Roles.UserId.Value)
                    .Include(u => u.ProfileImage)
                    .SingleOrDefaultAsync() ?? throw userNotFound;
        }

        userEntity.ProfileImage = Mapper.Map<UserImage>(image);
        var res = await Uow.Users.Update(userEntity);
        if (res) Uow.Save();

        return res;
    }
    public async Task<UserImageDto?> GetImage(ClaimsPrincipal user, int? id) {
        var role = await Roles.GetUserRole(user);
        var userNotFound = 
            new ExceptionModelBase((int)HttpStatusCode.BadRequest, "User don't have profile image. Please provide image!", "User Image", "Get Image");
        UserImageDto? userImage = null;

        if (role == UserRoles.User && id != null && Roles.UserId != id)
            throw new ExceptionModelBase((int)HttpStatusCode.Forbidden,
                "You try to edit other user image", "User image", "Update image");

        if (role == UserRoles.Administrator)
        {
            if (id != null)
                userImage = Mapper.Map<UserImageDto>((await Uow.Users.ReadById(id.Value)
                    .Include(u => u.ProfileImage)
                    .SingleOrDefaultAsync())?.ProfileImage ?? throw userNotFound);
        }
        if (role == UserRoles.User || id == null)
        {
            userImage = Mapper.Map<UserImageDto>((await Uow.Users.ReadById(Roles.UserId!.Value)
                .Include(u => u.ProfileImage)
                .SingleOrDefaultAsync())?.ProfileImage ?? throw userNotFound);
        }

        return userImage;
    }

}