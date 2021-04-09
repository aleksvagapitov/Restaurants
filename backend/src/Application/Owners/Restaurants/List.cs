using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Owners.Restaurants
{
    public class List
    {
        public class Query : IRequest<List<Restaurant>>
        {
        }

        public class Handler : IRequestHandler<Query, List<Restaurant>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly UserManager<AppUser> _userManager;
            public Handler(UserManager<AppUser> userManager, DataContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
                _userManager = userManager;
            }

            public async Task<List<Restaurant>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Include(x => x.Restaurants)
                                                .SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername());
                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized);

                var restaurants = await _context.Entry(user)
                                                .Collection(x => x.Restaurants)
                                                .Query()
                                                .Include(x => x.Categories)
                                                .ToListAsync();
                if (restaurants == null)
                    throw new RestException (HttpStatusCode.NotFound, new { restaurants = "Not found" });

                return user.Restaurants.ToList();
            }
        }
    }
}