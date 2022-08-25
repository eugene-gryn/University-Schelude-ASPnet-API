using AutoMapper;
using BLL.DTO.Models.JWTManager;
using BLL.DTO.Models.UserModels;
using DAL.Entities;
using DAL.UOW;
using Microsoft.Extensions.Logging.Abstractions;

namespace BLL.Services;

public class UserService : BaseService {
    public UserService(IUnitOfWork uow, IMapper mapper, IJwtManagerRepository jwt) : base(uow, mapper, jwt) { }

    public async Task<TokensDto?> Login(string username, string password) {
        var tokens = await JwtManager.CreateToken(new UserLoginDto {
            Login = username,
            Password = password
        });

        return tokens;
    }

    public async Task<UserRegisterDto?> Register(UserRegisterDto user) {
        var res = await Uow.Users.Add(Mapper.Map<User>(user));
        Uow.Save();

        if (!res) return null;
        
        return user;
    }
}