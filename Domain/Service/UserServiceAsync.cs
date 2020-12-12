using AutoMapper;
using DAL.Moldels;
using DAL.UnitOfWork;
using Domain.Service.Generic;
using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Service
{
    public class UserServiceAsync<Tv, Te> : GenericServiceAsync<Tv, Te>
                                        where Tv : UserViewModel
                                        where Te : User
    {
        //DI must be implemented specific service as well beside GenericAsyncService constructor
        public UserServiceAsync(IUnitOfWork unitOfWork, IMapper mapper)
        {
            if (_unitOfWork == null)
                _unitOfWork = unitOfWork;
            if (_mapper == null)
                _mapper = mapper;
        }

        //add here any custom service method or override genericasync service method
        //...
    }

}
