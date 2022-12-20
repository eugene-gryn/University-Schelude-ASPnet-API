using BLL.DTO.Models.JWTManager;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.JwtCustomValidate; 

public class ValidateModel {
    public static async Task<Task> ValidateUserModel(TokenValidatedContext context) {
        var jwtManager =
            context.HttpContext.RequestServices.GetService(typeof(IJwtManagerRepository)) as JwtManagerRepository;

        var user = context.Principal;
        if (user == null) context.Fail("Token is not provided!");

        if (jwtManager != null) {
            await ValidateModelExisting(context, jwtManager);
            await ValidateDateCreation(context, jwtManager);
        }

        return Task.CompletedTask;
    }


    private static Task ValidateModelExisting(TokenValidatedContext context, IJwtManagerRepository jwt) {
        if (context.Principal != null && !jwt.IsUserExist(context.Principal))
            context.Fail("User with this login is not exist!");

        return Task.CompletedTask;
    }
    private static async Task ValidateUserBanned(TokenValidatedContext context, IJwtManagerRepository jwt) {
        if (context.Principal != null && !(await jwt.IsUserBanned(context.Principal)))
            context.Fail("You are blocked from access to you account!");
    }

    private static Task ValidateDateCreation(TokenValidatedContext context, IJwtManagerRepository jwt) {
        if (context.Principal != null && !jwt.IsValidCreationDate(context.Principal)) context.Fail("Token is outdated!");

        return Task.CompletedTask;
    }


}