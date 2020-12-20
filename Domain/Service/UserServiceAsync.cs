using AutoMapper;
using DAL.Moldels;
using DAL.UnitOfWork;
using Domain.Service.Generic;
using Domain.Utilities;
using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Service
{
    public class UserServiceAsync<Tv, Te> : GenericServiceAsync<Tv, Te>
                                        where Tv : UserViewModel
                                        where Te : User
    {

        public UserServiceAsync(IUnitOfWork unitOfWork, IMapper mapper)
        {
            if (_unitOfWork == null)
                _unitOfWork = unitOfWork;
            if (_mapper == null)
                _mapper = mapper;
        }
        public async Task<int> Add(UserPassViewModel view)
        {
            if (UserHelper.IsValidData(view))
            {
                var users = await this.Get(x => x.Email == view.Email);
                if (users != null && users.Any())
                {
                    throw new Exception("User with the same email already exist");
                }
                var entity = _mapper.Map<Te>(source: view);
                await _unitOfWork.GetRepositoryAsync<Te>().Insert(entity);
                await _unitOfWork.SaveAsync();
                return entity.Id;
            }
            return -1;

        }
    }    
}
