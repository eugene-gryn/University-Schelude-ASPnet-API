using System.Security.Claims;
using BLL.DTO.Models.ExceptionBase;
using BLL.DTO.Models.UserModels;

namespace BLL.DTO.Models.JWTManager;

public interface IJwtManagerRepository {

    /// <summary>
    /// Find or generate token on DB on created user data
    /// <c>THROWS -> 403 Forbidden (wrong-credentials-login) - If login didn't contains in DB</c>
    /// </summary>
    /// <param name="user">User register validated data</param>
    /// <returns>
    /// Key: Token, Value: Refresh token DTO
    /// If token is null returns generated token
    /// And if token is exist return this token
    /// </returns>
    /// <exception cref="ExceptionModelBase"></exception>
    Task<KeyValuePair<string, TokensDto>> RegisterToken(UserLoginDto user);


    /// <summary>
    /// Create a new Token, that does not brake older tokens
    /// <c>THROWS -> 401 Unauthorized (wrong-refresh-token) - If provided refresh token is not correct</c>
    /// <c>THROWS -> 404 Not Found (wrong-user-id) - If user id is not related to any of the entities</c>
    /// </summary>
    /// <param name="id">User id in DB</param>
    /// <param name="refreshToken">User RT, that must be contains in DB</param>
    /// <returns>New Token for access :)</returns>
    /// <exception cref="ExceptionModelBase"></exception>
    /// <exception cref="ExceptionModelBase"></exception>
    Task<string> RefreshToken(int id, string refreshToken);


    /// <summary>
    /// Resets any older tokens and sets new RT for the User
    /// <c>THROWS -> 401 Unauthorized (wrong-refresh-token) - If provided refresh token is not correct</c>
    /// <c>THROWS -> 404 Not Found (wrong-user-id) - If user id is not related to any of the entities</c>
    /// </summary>
    /// <param name="id"></param>
    /// <param name="refreshToken"></param>
    /// <returns>New Set KeyValuePair, where Key is Token, Value is RT</returns>
    /// <exception cref="ExceptionModelBase"></exception>
    /// <exception cref="ExceptionModelBase"></exception>
    Task<KeyValuePair<string, TokensDto>> ResetToken(int id, string refreshToken);


    /// <summary>
    /// Gets from claims user name and find id that related to unique name in table!
    /// <c>THROWS -> 401 Unauthorized (wrong-token) - If token parsing get wrong!</c>
    /// </summary>
    /// <list type="bullet">
    /// </list>
    /// <param name="user">Token Claims</param>
    /// <returns>User Id that token related</returns>
    /// <exception cref="ExceptionModelBase"></exception>
    Task<int> GetUserId(ClaimsPrincipal user);


    /// <summary>
    /// Validate date creation
    /// </summary>
    /// <param name="user">Token Claims for Authorized User</param>
    /// <returns>True if creation date is correct</returns>
    bool IsValidCreationDate(ClaimsPrincipal user);


    /// <summary>
    /// Gets name from token and searching for this user in DB
    /// </summary>
    /// <param name="user">Token Claims for Authorized User</param>
    /// <returns>Returns true if user with this name is exist in DB</returns>
    bool IsUserExist(ClaimsPrincipal user);

    /// <summary>
    /// Gets info from DB to check if user has disabled status;
    /// </summary>
    /// <param name="user">Token Claims for Authorized User</param>
    /// <returns>Returns true if user enabled in DB</returns>
    Task<bool> IsUserBanned(ClaimsPrincipal user);
}