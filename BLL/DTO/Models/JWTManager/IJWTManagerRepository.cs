using System.Security.Claims;
using BLL.DTO.Models.UserModels;

namespace BLL.DTO.Models.JWTManager;

public interface IJwtManagerRepository {
    Task<TokensDto?> CreateToken(UserLoginDto user);

    Task<TokensDto?> RefreshToken(int id, string refreshToken);

    int GetUserId(ClaimsPrincipal user);
    string GetUserLogin(ClaimsPrincipal user);

}