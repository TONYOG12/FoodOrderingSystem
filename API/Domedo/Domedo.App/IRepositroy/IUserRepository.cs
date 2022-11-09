using Domedo.Domain.Models;
using Domedo.Domain.Requests.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.App.IRepositroy
{
    public interface IUserRepository
    {
        public Task<UserDto> CreateUser(UserCreateRequest model);
        public Task<IEnumerable<UserDto>> GetUsers();

    }
}
