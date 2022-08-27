using System.Security.Claims;
using BLL.DTO.Models.UserModels;

namespace BLL.DTO.Models.JWTManager;

public interface IJwtManagerRepository {
    Task<KeyValuePair<string, TokensDto>> RegisterToken(UserLoginDto user);

    Task<string> RefreshToken(int id, string refreshToken);
    Task<KeyValuePair<string, TokensDto>> ResetToken(int id, string refreshToken);

    Task<int> GetUserId(ClaimsPrincipal user);
    string GetUserLogin(ClaimsPrincipal user);

}