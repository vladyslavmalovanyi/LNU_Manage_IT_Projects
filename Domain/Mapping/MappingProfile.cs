using AutoMapper;
using DAL.Moldels;
using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;


namespace Domain.Mapping
{
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Create automap mapping profiles
        /// </summary>
        public MappingProfile()
        {


            CreateMap<UserViewModel, User>()
                .ForMember(dest => dest.Roles, opts => opts.MapFrom(src => string.Join(";", src.Roles)));
            CreateMap<User, UserViewModel>()
                .ForMember(dest => dest.Roles, opts => opts.MapFrom(src => src.Roles.Split(";", StringSplitOptions.RemoveEmptyEntries)));

            CreateMap<User, UserPassViewModel>()
                .ForMember(dest => dest.Password, opts => opts.MapFrom(src => src.DecryptedPassword));
            CreateMap<UserPassViewModel, User>()
                 .ForMember(dest => dest.DecryptedPassword, opts => opts.MapFrom(src => src.Password));

            CreateMap<Event, EventViewModel>();
            CreateMap<EventViewModel, Event>();
            CreateMap<List<Event>, List<EventViewModel>>().Include<PagedList<Event>, PagedList<EventViewModel>>();
            CreateMap<PagedList<Event>, PagedList<EventViewModel>>();


        }
    }
}

