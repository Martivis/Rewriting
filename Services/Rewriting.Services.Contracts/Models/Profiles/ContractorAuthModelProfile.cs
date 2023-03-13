using AutoMapper;
using Rewriting.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Contracts;

public class ContractorAuthModelProfile : Profile
{
    public ContractorAuthModelProfile()
    {
        CreateMap<Contract, ContractorAuthModel>();
    }
}
