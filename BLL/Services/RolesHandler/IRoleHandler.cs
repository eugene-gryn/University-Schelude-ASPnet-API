using System.Security.Claims;
using DAL.Entities;

namespace BLL.Services.RolesHandler;

public interface IRoleHandler {
    public int? UserId { get; }

    public Task<UserRoles> GetUserRole(ClaimsPrincipal user);
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