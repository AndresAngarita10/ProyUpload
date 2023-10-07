using API.Dtos;
using AutoMapper;
using Domain.Entities;

namespace ApiFarmacia.Profiles;

public class MappingProfiles : Profile
{

    public MappingProfiles()
    {
        CreateMap<FileUpload,FileUploadDto>().ReverseMap();
    }
}
