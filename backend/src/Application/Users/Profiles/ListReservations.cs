using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Application.Users.Profiles;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Users.Reservations
{
    public class ListReservations
    {
        public class Query : IRequest<List<UserReservationDto>>
        {
            public string Predicate { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<UserReservationDto>>
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

            public async Task<List<UserReservationDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Include(x => x.Reservations)
                                                .SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername());
                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized);

                var queryable = _context.Entry(user)
                                            .Collection(x => x.Reservations)
                                            .Query();

                switch (request.Predicate)
                {
                    case "past":
                        queryable = queryable.Where(a => a.DateTime <= DateTime.Now);
                        break;
                    default:
                        queryable = queryable.Where(a => a.DateTime >= DateTime.Now);
                        break;
                }

                var reservations = await queryable.ToListAsync();
                var reservationsToReturn = new List<UserReservationDto>();

                foreach (var reservation in reservations)
                {
                    var userReservation = new UserReservationDto
                    {
                        Id = reservation.Id,
                        RestaurantId = reservation.RestaurantId,
                        RestaurantName = _context.Restaurants.FindAsync(reservation.RestaurantId).Result.Name,
                        RestaurantPhoto = null,
                        DateTime = reservation.DateTime,
                        People = reservation.People,
                        Occasion = reservation.Occasion,
                        SpecialRequest = reservation.SpecialRequest,
                        Status = reservation.Status
                    };

                    reservationsToReturn.Add(userReservation);
                }


                return reservationsToReturn;
            }
        }
    }
}