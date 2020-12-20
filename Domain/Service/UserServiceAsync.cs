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
        public override async Task<int> Add(Tv view)
        {
            var users = await this.Get(x => x.Email == view.Email);
            if (users != null && users.Any())
            {
                throw new Exception("User with the same email already exist");
            }
            return await base.Add(view);
        }
        public async System.Threading.Tasks.Task<UserViewModel> AuthenticateAsync(LoginModel login)
        {

            var items = _mapper.Map <IEnumerable<UserPassViewModel>>(await _unitOfWork.GetRepositoryAsync<Te>().Get(x => x.Email == login.Email));
           
            var userView = items.SingleOrDefault();
            if (userView != null && userView.Password == login.Password)
            {
                return userView as Tv;
            }
            return null;
        }

    }

}
