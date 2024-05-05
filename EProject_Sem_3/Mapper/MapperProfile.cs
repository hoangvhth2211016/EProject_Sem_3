using AutoMapper;
using EProject_Sem_3.Models.Users;

namespace EProject_Sem_3.Mapper;

public class MapperProfile : Profile {
    public MapperProfile() {

        // map for user
        CreateMap<RegisterDto, User>();

    }
}