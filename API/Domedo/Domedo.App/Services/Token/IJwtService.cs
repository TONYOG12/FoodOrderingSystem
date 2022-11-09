using Domedo.Domain.Entities;
using Domedo.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.App.Services.Token
{
    public interface IJwtService
    {
        public LoginResponse Authenticate(User user, string clientId);
        public LoginResponse AuthenticateById(Guid id, string clientId);
    }
}
