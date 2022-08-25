using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BLL.DTO.Models.UserModels;
using BLL.DTO.Models.UserModels.Password;
using DAL.Entities;
using DAL.UOW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BLL.DTO.Models.JWTManager;

public class JwtManagerRepository : IJwtManagerRepository {
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _uow;

    public JwtManagerRepository(IConfiguration configuration, IUnitOfWork uow) {
        _configuration = configuration;
        _uow = uow;
    }

    public async Task<TokensDto?> CreateToken(UserLoginDto user) {
        var userLog = await _uow.Users.Read().Where(u => u.Login == user.Login).SingleOrDefaultAsync();

        if (userLog == null || !PasswordSingleton.Password.VerifyPassword(user.Password, userLog!.Password, userLog!.Salt)) return null;

        var created = DateTime.Now;
        var expires = DateTime.Now.AddDays(7);

        return new TokensDto {
            Token = GenerateToken(userLog),
            RefreshToken = GenerateRefreshToken(),
            TokenCreated = created,
            TokenExpires = expires
        };
    }

    public async Task<TokensDto?> RefreshToken(int id, string refreshToken) {
        var user = await _uow.Users.ReadById(id).SingleOrDefaultAsync();

        if (user == null) return null;

        var created = DateTime.Now;
        var expires = DateTime.Now.AddDays(7);

        return new TokensDto {
            Token = GenerateToken(user),
            RefreshToken = GenerateRefreshToken(),
            TokenCreated = created,
            TokenExpires = expires
        };
    }

    public int? GetUserId(ClaimsPrincipal user) {
        var claim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
        if (claim == null) return null;

        var v = claim.Value;

        if (string.IsNullOrEmpty(v)) return null;

        if (!int.TryParse(v, out var intId)) return null;

        return intId;
    }

    public string? GetUserLogin(ClaimsPrincipal user) {
        var claim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
        if (claim == null) return null;

        var login = claim.Value;

        if (string.IsNullOrEmpty(login)) return null;

        return login;
    }

    private string GenerateRefreshToken() {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    private string GenerateToken(User user) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = new UTF8Encoding().GetBytes(_configuration["JWT:Key"]);

        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(
                new[] {
                    new(ClaimTypes.Sid, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Login)
                }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha512Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);


        return tokenHandler.WriteToken(token);
    }
}