using System;
using AutoMapper;
using Contracts;
using MassTransit;
using SearchService.Consumers;
using SearchService.Models;

namespace SearchService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles(){
        CreateMap<AuctionCreated,Item>();
    }
}
