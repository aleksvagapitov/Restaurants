using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence;

namespace Application.User
{
    public class Login
    {
        public class Query : IRequest<UserDto>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator ()
            {
                RuleFor (x => x.Email).NotEmpty ();
                RuleFor (x => x.Password).NotEmpty ();
            }
        }

        public class Handler : IRequestHandler<Query, UserDto>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly SignInManager<AppUser> _signInManager;
            private readonly IJwtGenerator _jwtGenerator;
            public Handler (UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtGenerator jwtGenerator)
            {
                _jwtGenerator = jwtGenerator;
                _signInManager = signInManager;
                _userManager = userManager;

            }

            public async Task<UserDto> Handle (Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync (request.Email);
                if (user == null)
                    throw new RestException (HttpStatusCode.Unauthorized);

                var result = await _signInManager.CheckPasswordSignInAsync (user, request.Password, false);

                if (result.Succeeded)
                {
                    return new UserDto
                    {
                        DisplayName = user.DisplayName,
                            Username = user.UserName,
                            Token = _jwtGenerator.CreateToken (user),
                            Image = null
                            // userManager does not load Navigation properties
                            //Image = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
                    };
                }

                throw new RestException (HttpStatusCode.Unauthorized);
            }
        }
    }
}