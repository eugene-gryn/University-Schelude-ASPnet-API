using System.Security.Claims;
using DAL.Entities;

namespace BLL.Services.RolesHandler;

public interface IRoleHandler {
    public int? UserId { get; }
    /// <summary>
    /// Get user role from id user
    /// <c>THROWS -> 401 Unauthorized (wrong-token) - If token parsing get wrong!</c>
    /// </summary>
    public Task<UserRoles> GetUserRole(ClaimsPrincipal user);
    /// <summary>
    /// <c>THROWS -> 401 Unauthorized (wrong-token) - If token parsing get wrong!</c>
    /// <c>THROWS -> 404 Not Found (wrong-user-id) - If user id is not related to any of the entities</c>
    /// <c>THROWS -> 404 Not Found (wrong-entity-id) - If group with this is not found</c>
    /// </summary>
    public Task<UserGroupRoles> GetGroupUserRole(ClaimsPrincipal user, int groupId);
}

public enum UserRoles {
    User,
    Administrator
}

public enum UserGroupRoles {
    GroupMember,
    GroupModerator,
    GroupOwner
}