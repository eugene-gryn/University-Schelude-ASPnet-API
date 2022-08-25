using AutoMapper;
using BLL.DTO.Models.UserModels;
using BLL.DTO.Models.UserModels.Password;
using DAL.Entities;

namespace BLL.DTO.Mapper;

public class AutoMapperProfiles : Profile {
    public AutoMapperProfiles() {
        CreateMap<UserRegisterDto, User>()
            .ForMember(u => u.Salt,
                m => m.MapFrom(dto => dto.GetPasswordHash().Key))
            .ForMember(u => u.Password,
                m => m.MapFrom(dto => dto.GetPasswordHash().Value));

        CreateMap<Tokens, TokensDto>();
        CreateMap<TokensDto, Tokens>();
    }
}