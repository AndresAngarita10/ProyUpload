using API.Dtos;
using AutoMapper;
using Domain.Entities;

namespace API.profiles;
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        
        CreateMap<FileUpload,FileUploadDto>().ReverseMap();
    }
}
