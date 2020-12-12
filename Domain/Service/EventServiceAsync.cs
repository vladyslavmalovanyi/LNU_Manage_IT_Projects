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
    public class EventServiceAsync<Tv, Te> : GenericServiceAsync<Tv, Te>
                                                where Tv : EventViewModel
                                                where Te : Event
    {
        //DI must be implemented specific service as well beside GenericAsyncService constructor
        public EventServiceAsync(IUnitOfWork unitOfWork, IMapper mapper)
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
