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
        await Uow.Users.Update(user);
        Uow.Save();

        return tokens;
    }

    public async Task<UserRegisterDto?> Register(UserRegisterDto user) {
        var res = await Uow.Users.Add(Mapper.Map<User>(user));

        Uow.Save();

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

    public async Task<List<UserWithoutCollectionsDto>> GetUsers(ClaimsPrincipal user, int offset = 0, int limit = 10) {
        var role = await Roles.GetUserRole(user);


        if (role == UserRoles.Administrator)
            return Uow.Users.Read().Where((user1) => user1.Id > offset).Take(limit).AsEnumerable()
                .Select(u => Mapper.Map<UserWithoutCollectionsDto>(u)).ToList();
        
        return Uow.Users.ReadById(await JwtManager.GetUserId(user)).AsEnumerable()
            .Select(u => Mapper.Map<UserWithoutCollectionsDto>(u)).ToList();
    }
}