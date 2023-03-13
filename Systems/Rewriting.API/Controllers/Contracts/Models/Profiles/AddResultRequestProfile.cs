using AutoMapper;
using Rewriting.Services.Contracts;

namespace Rewriting.API.Controllers.Contracts;

public class AddResultRequestProfile : Profile
{
    public AddResultRequestProfile()
    {
        CreateMap<AddResultRequest, AddResultModel>();
    }
}

