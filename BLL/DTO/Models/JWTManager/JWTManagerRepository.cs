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
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public JwtManagerRepository(IConfiguration configuration, IUnitOfWork uow, IMapper mapper) {
        _configuration = configuration;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<string> RefreshToken(int id, string refreshToken) {
        var user = await _uow.Users.ReadById(id).SingleOrDefaultAsync();

        if (user == null) throw new ExceptionModelBase(400, "User with this id is not found", "User", "Refresh token");
        if (user.Token.RefreshToken != refreshToken)
            throw new ExceptionModelBase(401, "Bad refresh token", "User", "Refresh token");

        user.Token.TokenExpires = DateTime.UtcNow.AddDays(7);
        await _uow.Users.Update(user);
        _uow.Save();

        return GenerateToken(user);
    }

    public async Task<KeyValuePair<string, TokensDto>> ResetToken(int id, string refreshToken) {
        var user = await _uow.Users.ReadById(id).SingleOrDefaultAsync();

        if (user == null) throw new ExceptionModelBase(400, "User with this id is not found", "User", "Refresh token");
        if (user.Token.RefreshToken != refreshToken)
            throw new ExceptionModelBase(401, "Bad refresh token", "User", "Refresh token");

        user.Token.TokenCreated = DateTime.UtcNow;
        user.Token.TokenExpires = DateTime.UtcNow.AddDays(7);
        user.Token.RefreshToken = GenerateRefreshToken();
        await _uow.Users.Update(user);
        _uow.Save();

        return new KeyValuePair<string, TokensDto>(GenerateToken(user), _mapper.Map<TokensDto>(user.Token));
    }

    public async Task<int> GetUserId(ClaimsPrincipal user) {
        var login = GetUserLogin(user);
        return await _uow.Users.Read().Where(u => u.Login == login).Select(u => u.Id).SingleAsync();
    }

    public string GetUserLogin(ClaimsPrincipal user) {
        var claim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
        var expiredClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Expired);

        var exceptionNotFound = new ExceptionModelBase(400, "Wrong token, try to relogin or refresh token!",
            "User token", "Get user login from logged token");

        if (claim == null || expiredClaim == null)
            throw exceptionNotFound;

        var login = claim.Value;
        var createdDate = expiredClaim.Value ?? string.Empty;

        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(createdDate))
            throw exceptionNotFound;

        if (_uow.Users.Read()
            .Where(u => u.Login == login)
            .Include(u => u.Token)
            .Select(u => u.Token)
            .Select(t => t.TokenCreated)
            .ToList()
            .All(created =>
                created.ToString("MM/dd/yyyy HH:mm:ss") != createdDate))
            throw exceptionNotFound;

        return login;
    }

    public async Task<KeyValuePair<string, TokensDto>> RegisterToken(UserLoginDto user) {
        var userLog = await _uow.Users.Read().Where(u => u.Login == user.Login)
            .Include(u => u.Token)
            .SingleOrDefaultAsync();

        if (userLog == null ||
            !PasswordSingleton.Password.VerifyPassword(user.Password, userLog!.Password, userLog!.Salt))
            throw new ExceptionModelBase(401, "Wrong login or password", "User", "Login user token");

        if (string.IsNullOrEmpty(userLog.Token.RefreshToken)) {
            userLog.Token.TokenCreated = DateTime.UtcNow;
            userLog.Token.TokenExpires = DateTime.UtcNow.AddDays(7);

            return new KeyValuePair<string, TokensDto>(GenerateToken(userLog), new TokensDto {
                RefreshToken = GenerateRefreshToken(),
                TokenCreated = userLog.Token.TokenCreated,
                TokenExpires = userLog.Token.TokenExpires
            });
        }

        return new KeyValuePair<string, TokensDto>(GenerateToken(userLog), _mapper.Map<TokensDto>(userLog.Token));
    }


    private string GenerateRefreshToken() {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    private string GenerateToken(User user) {
        var claims = new List<Claim> {
            new(ClaimTypes.Name, user.Login),
            new(ClaimTypes.Expired, user.Token.TokenCreated.ToString("MM/dd/yyyy HH:mm:ss"))
        };

        var secKey = _configuration["JWT:Key"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secKey));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: user.Token.TokenExpires,
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}