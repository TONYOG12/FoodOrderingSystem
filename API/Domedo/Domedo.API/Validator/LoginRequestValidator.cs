using Domedo.Domain.Context;
using Domedo.Domain.Entities;
using Domedo.Domain.Requests.Auth;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Domedo.API.Validator
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator(DomedoDbContext context)
        {
            RuleFor(item => item.Email)
                .Must((request, s) =>
                {
                    var user = context.Users.FirstOrDefault(item =>/* item.UserName == s ||*/ item.Email == s);
                    var passwordHash = new PasswordHasher<User>();
                    return user != null && user.DeletedAt == null &&
                           passwordHash.VerifyHashedPassword(user, user.PasswordHash, request.Password) ==
                           PasswordVerificationResult.Success;

                })
                .WithMessage("Your credentials do not match our records");

        }
    }
}
