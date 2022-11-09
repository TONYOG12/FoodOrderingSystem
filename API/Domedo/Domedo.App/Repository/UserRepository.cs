using AutoMapper;
using Domedo.App.IRepositroy;
using Domedo.App.Services.Token;
using Domedo.Domain.Context;
using Domedo.Domain.Entities;
using Domedo.Domain.Models;
using Domedo.Domain.Requests.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.App.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly DomedoDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public UserRepository(UserManager<User> userManager, DomedoDbContext context, IJwtService jwtService, IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        public async Task<UserDto> CreateUser(UserCreateRequest model)
        {

            var newUser = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Phone,
                UserName = model.Email,
                Email = model.Email,
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                throw new ArgumentException("we could not create a user");
            }
            var dto = _mapper.Map<UserDto>(newUser);

            await _context.SaveChangesAsync();

            return dto;
        }
        public async Task<IEnumerable<UserDto>> GetUsers()
        {

            var users = await _context.Users.OrderBy(item => item.UserName).ToListAsync();

            var dto = _mapper.Map<IEnumerable<UserDto>>(users);

            return dto;
        }
    }
}
