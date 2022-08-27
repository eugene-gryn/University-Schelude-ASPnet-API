using AutoMapper;
using BLL.DTO.Models.JWTManager;
using BLL.Services.RolesHandler;
using DAL.UOW;

namespace BLL.Services;

public abstract class BaseService {
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