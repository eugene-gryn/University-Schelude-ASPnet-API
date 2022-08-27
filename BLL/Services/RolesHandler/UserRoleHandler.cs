using System.Net;
using System.Security.Claims;
using BLL.DTO.Models.ExceptionBase;
using BLL.DTO.Models.JWTManager;
using DAL.Entities;
using DAL.UOW;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.RolesHandler;

public class UserRoleHandler : IRoleHandler {
    private readonly IUnitOfWork _uow;
    private readonly IJwtManagerRepository _jwtManager;

    public UserRoleHandler(IUnitOfWork uow, IJwtManagerRepository jwtManager) {
        _uow = uow;
        _jwtManager = jwtManager;
    }

    public async Task<UserRoles> GetUserRole(ClaimsPrincipal user) {
        var id = await _jwtManager.GetUserId(user);

        if (_uow.Users.ReadById(id).Any(u => u.IsAdmin)) return UserRoles.Administrator;

        return UserRoles.User;
    }

    public async Task<UserGroupRoles> GetGroupUserRole(ClaimsPrincipal user, int groupId) {
        var id = await _jwtManager.GetUserId(user);

        if (await _uow.Groups.ReadById(groupId).AnyAsync())
            throw new ExceptionModelBase((int)HttpStatusCode.NotFound, "Group with this id does not exist", "Group",
                "User group role");

        var userRoles = await _uow.Users.ReadById(id)
            .Include(u => u.UsersRoles)
            .SingleAsync();

        var role = userRoles.UsersRoles.SingleOrDefault(r => r.GroupId == groupId);

        if(role == null) throw new ExceptionModelBase((int)HttpStatusCode.NotFound, "User does not consist in this group!", "User roles",
            "User group role");

        if (role.IsOwner) return UserGroupRoles.GroupOwner;

        if (role.IsModerator) return UserGroupRoles.GroupModerator;

        return UserGroupRoles.GroupMember;
    }

}