using AutoMapper;
using Azure.Models;
using Services.Models;
using System;

namespace Azure.Mappings
{
    public class UserMapping : Profile
    {
        public UserMapping( )
        {
            CreateMap<Address, AddressDto>();
            CreateMap<AddressDto, Address>();

            CreateMap<User, UserDto>()
                .ForMember(x => x.FullName, opt => opt.MapFrom(y => $"{y.FirstName} {y.LastName}"));
            CreateMap<UserDto, User>()
                 .ForMember(x => x.FirstName, opt => opt.MapFrom(y => GetNpart(y.FullName, 0)))
                 .ForMember(x => x.LastName, opt => opt.MapFrom(y => GetNpart(y.FullName, 1)));
        }

        private string GetNpart(string str, int n)
        {
            var part = "";
            if (!String.IsNullOrWhiteSpace(str))
            {
                part = str.Split(" ")[n];
            }
            return part;
        }
    }

}