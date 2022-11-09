using Domedo.App.Extensions;
using Domedo.App.IRepositroy;
using Domedo.App.Services.Token;
using Domedo.App.Utils;
using Domedo.Domain.Context;
using Domedo.Domain.Entities;
using Domedo.Domain.Requests.Auth;
using Domedo.Domain.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.App.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IJwtService _jwtService;
        private readonly UserManager<User> _userManager;

        public AuthRepository(IJwtService jwtService, UserManager<User> userManager)
        {
            _jwtService = jwtService;
            _userManager = userManager;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null || user.IsDeleted())
            {
                throw new AuthenticationException("Wrong login details");
            }

            var response = _jwtService.Authenticate(user, request.ClientId);
            return response;


        }

    }
}
