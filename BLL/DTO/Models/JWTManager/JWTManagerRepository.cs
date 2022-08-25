using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using BLL.DTO.Models.ExceptionBase;
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
    private readonly IMapper _mapper;

    public JwtManagerRepository(IConfiguration configuration, IUnitOfWork uow, IMapper mapper) {
        _configuration = configuration;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<TokensDto?> CreateToken(UserLoginDto user) {
        var userLog = await _uow.Users.Read().Where(u => u.Login == user.Login)
            .Include(u => u.Token)
            .SingleOrDefaultAsync();

        if (userLog == null || !PasswordSingleton.Password.VerifyPassword(user.Password, userLog!.Password, userLog!.Salt))
            throw new ExceptionModelBase(401, "Wrong login or password", "User", "Login user token");

        if (String.IsNullOrEmpty(userLog.Token.Token)) {
            var created = DateTime.Now;
            var expires = DateTime.Now.AddDays(7);

            return new TokensDto {
                Token = GenerateToken(userLog),
                RefreshToken = GenerateRefreshToken(),
                TokenCreated = created,
                TokenExpires = expires
            };
        }
        
        return _mapper.Map<TokensDto>(userLog.Token);

    }

    public async Task<TokensDto?> RefreshToken(int id, string refreshToken) {
        var user = await _uow.Users.ReadById(id).SingleOrDefaultAsync();

        if (user == null) throw new ExceptionModelBase(400, "User with this id is not found", "User", "Refresh token");

        var created = DateTime.Now;
        var expires = DateTime.Now.AddDays(7);

        return new TokensDto {
            Token = GenerateToken(user),
            RefreshToken = GenerateRefreshToken(),
            TokenCreated = created,
            TokenExpires = expires
        };
    }

    public int GetUserId(ClaimsPrincipal user) {
        var claim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);

        var exceptionNotFound = new ExceptionModelBase(400, "Wrong token, try to relogin or refresh token!", "User token", "Get user id from logged token");

        if (claim == null)
            throw exceptionNotFound;
        var v = claim.Value;

        if (string.IsNullOrEmpty(v))
            throw exceptionNotFound;

        if (!int.TryParse(v, out var intId))
            throw exceptionNotFound;

        return intId;
    }

    public string GetUserLogin(ClaimsPrincipal user) {
        var claim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

        var exceptionNotFound = new ExceptionModelBase(400, "Wrong token, try to relogin or refresh token!", "User token", "Get user login from logged token");

        if (claim == null) 
            throw exceptionNotFound;

        var login = claim.Value;

        if (string.IsNullOrEmpty(login))
            throw exceptionNotFound;

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
                    new Claim(ClaimTypes.Sid, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Login),
                }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha512Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);


        return tokenHandler.WriteToken(token);
    }
}