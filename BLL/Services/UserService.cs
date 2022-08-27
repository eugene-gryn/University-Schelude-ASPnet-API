using System.Security.Claims;
using AutoMapper;
using BLL.DTO.Models.ExceptionBase;
using BLL.DTO.Models.JWTManager;
using BLL.DTO.Models.UserModels;
using DAL.Entities;
using DAL.UOW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BLL.Services;

public class UserService : BaseService {
    private IConfiguration _conf;

    public UserService(IUnitOfWork uow, IMapper mapper, IJwtManagerRepository jwt, IConfiguration conf) : base(uow,
        mapper, jwt) {
        _conf = conf;
    }

    public async Task<KeyValuePair<string, TokensDto>> Login(string username, string password) {
        var tokens = await JwtManager.RegisterToken(new UserLoginDto {
            Login = username,
            Password = password
        });

        if (tokens.Value == null)
            throw new ExceptionModelBase(401, "Wrong login or password", "User", "Login tokenUser token");

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

        if (!res) throw new ExceptionModelBase(409, "Login already used", "User", "Register new tokenUser");

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

    public async Task<List<UserRegisterDto>> GetUsers(ClaimsPrincipal user) {
        var userLogin = JwtManager.GetUserLogin(user);

        var userEntity = await Uow.Users.Read().Where(u => u.Login == userLogin).FirstOrDefaultAsync();

        return new List<UserRegisterDto>();
    }
}