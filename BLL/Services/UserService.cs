using AutoMapper;
using BLL.DTO.Models.JWTManager;
using BLL.DTO.Models.UserModels;
using BLL.DTO.Models.UserModels.Password;
using DAL.UOW;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BLL.Services;

public class UserService : BaseService {
    public UserService(IUnitOfWork uow, IMapper mapper, IJwtManagerRepository jwt) : base(uow, mapper, jwt) {
    }

    public async Task<TokensDTO?> Login(string username, string password) {
        var tokens = await JwtManager.CreateToken(new UserLoginDto()
        {
            Login = username,
            Password = password
        });

        return tokens;

    }
}