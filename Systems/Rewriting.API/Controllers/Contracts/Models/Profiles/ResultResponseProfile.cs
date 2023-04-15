using AutoMapper;
using Rewriting.Services.Contracts;

namespace Rewriting.API.Controllers.Contracts;

public class ResultResponseProfile : Profile
{
    public ResultResponseProfile()
    {
        CreateMap<ResultModel, ResultResponse>();
    }
}
