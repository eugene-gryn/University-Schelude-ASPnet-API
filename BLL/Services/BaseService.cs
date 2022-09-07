using System.Net;
using AutoMapper;
using BLL.DTO.Models.ExceptionBase;
using BLL.DTO.Models.JWTManager;
using BLL.Services.RolesHandler;
using DAL.UOW;

namespace BLL.Services;

public abstract class BaseService {
    /// <summary>
    /// 404 NotFound (wrong-user-id)
    /// </summary>
    public static readonly ExceptionModelBase UserNotFound = new(HttpStatusCode.NotFound, ErrorTypes.WrongUserId,
        "User with this id wasn't found :(");
    /// <summary>
    /// 404 Not Found (wrong-entity-id)
    /// </summary>
    public static readonly ExceptionModelBase EntityNotFound = new(HttpStatusCode.NotFound, ErrorTypes.WrongId,
        "Requested entity with this id does not found in table :(");
    /// <summary>
    /// 403 Forbidden (invalid-operation-access-to-user)
    /// </summary>
    public static readonly ExceptionModelBase NotThisUser = new(HttpStatusCode.Forbidden, ErrorTypes.WrongAccessUser,
        "You trying to get or edit information about other users! :(");

    protected BaseService(IUnitOfWork uow, IMapper mapper, IJwtManagerRepository jwtManager, IRoleHandler roles) {
        JwtManager = jwtManager;
        Roles = roles;
        Uow = uow;
        Mapper = mapper;
    }

    protected IJwtManagerRepository JwtManager { get; }
    public IRoleHandler Roles { get; }

    protected IUnitOfWork Uow { get; }
    protected IMapper Mapper { get; }
}