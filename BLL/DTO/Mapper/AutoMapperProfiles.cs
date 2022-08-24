using AutoMapper;
using BLL.DTO.Models.UserModels;
using DAL.Entities;

namespace BLL.DTO.Mapper;

public class AutoMapperProfiles : Profile {
    public AutoMapperProfiles() {

        CreateMap<User, UserLoginDto>();
        CreateMap<UserLoginDto, User>();
    }
}