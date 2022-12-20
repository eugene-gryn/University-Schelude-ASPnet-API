using BLL.Services.RolesHandler;
using DAL.Entities;

namespace BLL.DTO.Models.JWTManager;

internal static class UserRole {
    public static UserRoles GerRole(this User user) {
        return user.IsAdmin ? UserRoles.Administrator : UserRoles.User;
    }

    public static string RoleString(this UserRoles role) {
        return role switch {
            UserRoles.Administrator => "Administrator",
            _ => "User"
        };
    }
}