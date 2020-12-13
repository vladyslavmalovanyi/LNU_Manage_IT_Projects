using AutoMapper;
using DAL.Moldels;
using DAL.UnitOfWork;
using Domain.Service.Generic;
using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public virtual async Task<PagedList<Tv>> GetPagination(PageParameters parameters)
        {
            var entities = await PagedList<Te>.ToPagedListAsync( _unitOfWork.GetRepository<Te>().FindAll(),
                parameters.PageNumber,
           parameters.PageSize);

            return new PagedList<Tv>(
                _mapper.Map<IEnumerable<Tv>>(source: entities.ToList()).ToList(),
                entities.Count, 
                entities.CurrentPage, 
                entities.PageSize);
        }

        public virtual async Task<PagedList<Tv>> GetSearch(SearchParameters searchParameters, PageParameters parameters)
        {
           
            var entities = await PagedList<Te>.ToPagedListAsync(_unitOfWork.GetRepository<Te>().Get(
                x => x.Name.Contains(searchParameters.Name) 
                && x.DateStart>=searchParameters.DateTimeStart 
                && x.DateFinish <= searchParameters.DateTimeFinish), 
                parameters.PageNumber,
                parameters.PageSize);

            return new PagedList<Tv>(
                _mapper.Map<IEnumerable<Tv>>(source: entities.ToList()).ToList(),
                entities.Count,
                entities.CurrentPage,
                entities.PageSize);
        }
        //add here any custom service method or override genericasync service method
        //...
    }
}
