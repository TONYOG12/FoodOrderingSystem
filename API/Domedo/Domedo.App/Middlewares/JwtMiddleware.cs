using Domedo.Domain.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.App.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, DomedoDbContext db)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrWhiteSpace(token))
                await AttachUserToContext(context, token, db);

            await _next(context);
        }

        private static async Task AttachUserToContext(HttpContext context, string token, DomedoDbContext db)
        {
            try
            {
                if (token != null)
                {
                    var jwtToken = new JwtSecurityToken(token);
                    var user = await db.Users.FirstOrDefaultAsync(item => item.Id == Guid.Parse(jwtToken.Subject));

                    if (user != null)
                    {
                        context.Items["Sub"] = jwtToken.Subject;
                    }
                }
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}
