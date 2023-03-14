﻿using AutoMapper;
using Rewriting.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Contracts;

public class ClientAuthModelProfile : Profile
{
    public ClientAuthModelProfile()
    {
        CreateMap<Contract, ClientAuthModel>()
            .ForMember(t => t.ClientUid, o => o.MapFrom(s => s.Order.ClientUid));
    }
}
