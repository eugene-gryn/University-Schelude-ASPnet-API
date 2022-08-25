using System.Security.Claims;
using AutoMapper;
using BLL.DTO.Models.ExceptionBase;
using BLL.DTO.Models.JWTManager;
using BLL.DTO.Models.UserModels;
using DAL.Entities;
using DAL.UOW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace BLL.Services;

public class UserService : BaseService {
    public UserService(IUnitOfWork uow, IMapper mapper, IJwtManagerRepository jwt) : base(uow, mapper, jwt) { }

    public async Task<TokensDto?> Login(string username, string password) {
        var tokens = await JwtManager.CreateToken(new UserLoginDto {
            Login = username,
            Password = password
        });

        if (tokens == null) throw new ExceptionModelBase(401, "Wrong login or password", "User", "Login tokenUser token");

        var user = await Uow.Users.Read().Where(u => u.Login == username)
            .Include(u => u.Token)
            .SingleOrDefaultAsync();

        user!.Token = Mapper.Map<Tokens>(tokens);
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

    public async Task<TokensDto?> TokenUpdate(ClaimsPrincipal tokenUser, string refreshToken) {
        var userId = JwtManager.GetUserId(tokenUser);

        var newTokens = await JwtManager.RefreshToken(userId, refreshToken);

        var user = await Uow.Users.ReadById(userId)
            .Include(u => u.Token)
            .SingleOrDefaultAsync();

        user!.Token = Mapper.Map<Tokens>(newTokens);
        await Uow.Users.Update(user);
        Uow.Save();

        return newTokens;
    } 
}