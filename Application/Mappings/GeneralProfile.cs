using Application.Models;
using AutoMapper;
using Domain.Entities;
using PolicyManagement.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<PolicyViewModel, Policy>();
            CreateMap<CustomerViewModel, Customer>().ReverseMap();
            CreateMap<Policy, GetPolicyViewModel>();
        }
    }
}
