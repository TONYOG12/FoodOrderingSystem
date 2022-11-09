using Domedo.App.Exceptions;
using Domedo.Domain.Context;
using Domedo.Domain.Entities;
using Domedo.Domain.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.App.Services.Token
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly DomedoDbContext _context;

        public JwtService(IConfiguration configuration, DomedoDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public LoginResponse Authenticate(User user, string clientId)
        {
            string tokenJson = GenerateToken(user, clientId);
            string refreshToken = SaveRefreshToken(user.Id);
            var expiry = DateTime.Now.AddMonths(6);


            return new LoginResponse
            {
                AccessToken = tokenJson,
                RefreshToken = refreshToken,
                ExpiresIn = Convert.ToInt32(expiry.Subtract(DateTime.Now).TotalSeconds)
            };

        }


        private static string GenerateRefreshToken(int size = 32)
        {
            var randomNumber = new byte[size];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string SaveRefreshToken(Guid userId)
        {
            var refreshToken = GenerateRefreshToken();
            _context.RefreshTokens.Add(new RefreshToken
            {
                UserId = userId,
                Token = refreshToken,
                Expiry = DateTime.Now.AddMonths(6),
                CreatedAt = DateTime.Now

            });
            _context.SaveChanges();

            return refreshToken;
        }


        private string GenerateToken(User user, string clientId)
        {
            var jwtKey = _configuration.GetValue<string>("JwtSettings:Key");
            var keyBytes = Encoding.ASCII.GetBytes(jwtKey);

            var tokenHandler = new JwtSecurityTokenHandler();

            var expiry = DateTime.Now.AddMonths(6);

            var roleIds = _context.UserRoles.Where(item => item.UserId == user.Id).Select(item => item.RoleId).ToList();
            var roles = _context.Roles.Where(item => roleIds.Contains(item.Id)).Select(item => new { item.DisplayName, item.Id }).ToList();

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new("client_id", clientId ),
                    new(ClaimTypes.NameIdentifier, user.UserName),
                    new("firstName", user.FirstName),
                    new("lastName", user.LastName),
                    new("roles", JsonConvert.SerializeObject(roles))

                }),
                Expires = expiry,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256

                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenJson = tokenHandler.WriteToken(token);
            return tokenJson;
        }

        public LoginResponse AuthenticateById(Guid id, string clientId)
        {
            var user = _context.Users.Find(id.ToString());
            if (user != null)
            {
                return Authenticate(user, clientId);
            }

            throw new ModelNotFoundException();
        }
    }
}
