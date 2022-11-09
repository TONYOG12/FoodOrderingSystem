using Domedo.Domain.Requests.Auth;
using Domedo.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.App.IRepositroy
{
    public interface IAuthRepository
    {
        public Task<LoginResponse> Login(LoginRequest request);

    }
}
