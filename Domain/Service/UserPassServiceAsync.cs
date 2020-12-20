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
    public class UserPassServiceAsync<Tv, Te> : GenericServiceAsync<Tv, Te>
                                       where Tv : UserPassViewModel
                                       where Te : User
        {

            public UserPassServiceAsync(IUnitOfWork unitOfWork, IMapper mapper)
            {
                if (_unitOfWork == null)
                    _unitOfWork = unitOfWork;
                if (_mapper == null)
                    _mapper = mapper;
            }

            public async System.Threading.Tasks.Task<Tv> AuthenticateAsync(LoginModel login)
            {

                var items = _mapper.Map<IEnumerable<UserPassViewModel>>(await _unitOfWork.GetRepositoryAsync<Te>().Get(x => x.Email == login.Email));

                var userView = items.SingleOrDefault();
                if (userView != null && userView.Password == login.Password)
                {
                    return userView as Tv;
                }
                return null;
            }

        }

    }

