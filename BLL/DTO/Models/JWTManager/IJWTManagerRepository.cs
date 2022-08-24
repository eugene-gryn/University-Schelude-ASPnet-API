using System.Security.Claims;
using BLL.DTO.Models.UserModels;

namespace BLL.DTO.Models.JWTManager;

public interface IJwtManagerRepository {
    Task<TokensDTO?> CreateToken(UserLoginDto user);

    Task<TokensDTO?> RefreshToken(int id, string refreshToken);

    int? GetUserId(ClaimsPrincipal user);
    string? GetUserLogin(ClaimsPrincipal user);

}