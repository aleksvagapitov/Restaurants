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

namespace Application.Owners.Reservations
{
    public class List
    {
        public class Query : IRequest<List<Reservation>>
        {
            public Guid RestaurantId { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<Reservation>>
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

            public DataContext Context => _context;

            public async Task<List<Reservation>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Include(x => x.Restaurants)
                                                    .ThenInclude(x => x.Reservations)
                                                .SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername());
                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized);

                var reservations = await _context.Entry(user)
                                                .Collection(x => x.Restaurants)
                                                .Query()
                                                .Where(x => x.Id == request.RestaurantId)
                                                .SelectMany(x => x.Reservations)
                                                .ToListAsync();
                if (reservations == null)
                    throw new RestException (HttpStatusCode.NotFound, new { reservations = "Not found" });

                return reservations;
            }
        }
    }
}