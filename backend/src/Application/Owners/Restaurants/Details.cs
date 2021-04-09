using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Owners.Restaurants
{
    public class Details
    {
        public class Query : IRequest<Restaurant>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Restaurant>
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

            public async Task<Restaurant> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Include(x => x.Restaurants)
                                                    .ThenInclude(x => x.Reservations)
                                                .Include(x => x.Restaurants)
                                                    .ThenInclude(x => x.Categories)
                                                .SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername());
                if (user == null)
                    throw new RestException (HttpStatusCode.Unauthorized);

                var restaurant = await _context.Entry(user)
                                                .Collection(x => x.Restaurants)
                                                .Query()
                                                .Include(x => x.Photos)
                                                .Include(x => x.Categories)
                                                .SingleOrDefaultAsync(x => x.Id == request.Id);

                if (restaurant == null)
                    throw new RestException(HttpStatusCode.NotFound, new { restaurant = "Not found" });
        
                return restaurant;
            }
        }
    }
}