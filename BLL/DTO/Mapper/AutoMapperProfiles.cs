using AutoMapper;
using BLL.DTO.Models.CoupleModels;
using BLL.DTO.Models.GroupsModels;
using BLL.DTO.Models.HomeworkTaskModels;
using BLL.DTO.Models.SubjectModels;
using BLL.DTO.Models.UserModels;
using DAL.Entities;

namespace BLL.DTO.Mapper;

public class AutoMapperProfiles : Profile {
    public AutoMapperProfiles() {
        #region User

        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();


        CreateMap<Settings, SettingsDto>();
        CreateMap<SettingsDto, Settings>();

        // Register 
        CreateMap<UserRegisterDto, User>()
            .ForMember(u => u.Salt,
                m => m.MapFrom(dto => dto.GetPasswordHash().Key))
            .ForMember(u => u.Password,
                m => m.MapFrom(dto => dto.GetPasswordHash().Value));

        // Tokens for logging
        CreateMap<Tokens, TokensDto>();
        CreateMap<TokensDto, Tokens>();

        // For user list
        CreateMap<User, UserInfoDto>();

        // For user image
        CreateMap<UserImageDto, UserImage>()
            .ForMember(u => u.Image,
                m => m.MapFrom(dto => dto.Image.ToArray()));
        CreateMap<UserImage, UserImageDto>()
            .ForMember(u => u.Image,
                i => i.MapFrom(entity => new MemoryStream(entity.Image)));

        #endregion


        #region Group

        CreateMap<UserRole, UserRoleDto>();
        CreateMap<UserRoleDto, UserRole>();

        CreateMap<Group, GroupDto>();
        CreateMap<GroupDto, Group>();

        #endregion


        #region Couple

        CreateMap<Couple, CoupleDto>();
        CreateMap<CoupleDto, Couple>();

        #endregion


        #region Subject

        CreateMap<Subject, SubjectDto>();
        CreateMap<SubjectDto, Subject>();

        #endregion


        #region Homework

        CreateMap<HomeworkTask, HomeworkTaskDto>();
        CreateMap<HomeworkTaskDto, HomeworkTask>();

        #endregion
    }
}