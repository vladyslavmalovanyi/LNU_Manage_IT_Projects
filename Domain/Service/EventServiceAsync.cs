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
        public override async Task<int> Add(Tv view)
        {
            if (view.DateFinish < view.DateStart) // validation 
            {
                throw new Exception("Date finish < Date start");
            }
            var user = await _unitOfWork.GetRepositoryAsync<User>().GetOne(predicate: x => x.Id == view.CreatorId);
            if (user == null)
            {
                throw new Exception("User is null");
            }
            return await base.Add(view);
        }
        public virtual async Task<int> Remove(int id, int idUser)
        {
            Te entity = await _unitOfWork.Context.Set<Te>().FindAsync(id);
            if (entity != null && entity.CreatorId != idUser)
            {
                throw new Exception("You aren't creator of this event");
            }
            await _unitOfWork.GetRepositoryAsync<Te>().Delete(id);
            return await _unitOfWork.SaveAsync();
        }

        public virtual async Task<int> Update(Tv view, int idUser)
        {
            Te entity = await _unitOfWork.Context.Set<Te>().FindAsync(view.Id);
            if (entity != null && entity.CreatorId != idUser)
            {
                throw new Exception("You aren't creator of this event");
            }
            return await base.Update(view);
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
    }
}
