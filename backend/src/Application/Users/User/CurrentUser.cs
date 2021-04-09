using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.User
{
    public class CurrentUser
    {
        public class Query : IRequest<UserDto> { }

        public class Handler : IRequestHandler<Query, UserDto>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IUserAccessor _userAccessor;
            private readonly DataContext _context;
            public Handler (UserManager<AppUser> userManager, IJwtGenerator jwtGenerator, 
                            IUserAccessor userAccessor, DataContext context)
            {
                _userAccessor = userAccessor;
                _jwtGenerator = jwtGenerator;
                _userManager = userManager;
                _context = context;
            }

            public async Task<UserDto> Handle (Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync (_userAccessor.GetCurrentUsername ());
                var photos = await _context.Entry(user)
                                        .Collection(x => x.Photos)
                                        .Query()
                                        .FirstOrDefaultAsync(x => x.IsMain);

                return new UserDto
                {
                    DisplayName = user.DisplayName,
                        Username = user.UserName,
                        Token = _jwtGenerator.CreateToken (user),
                        Image = photos?.Url
                };
            }
        }
    }
}