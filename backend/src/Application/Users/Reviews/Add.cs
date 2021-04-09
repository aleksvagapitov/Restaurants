using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Users.Reviews
{
    public class Add
    {
        public class Command : IRequest
        {
            public Guid RestaurantId { get; set; }
            public int Stars { get; set; }
            public string Body { get; set; }
            public DateTime CreatedAt { get; set; }
            public Guid ReservationId { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Include(x => x.Reviews)
                        .SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername());

                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized);

                var userReview = new Review
                {
                    RestaurantId = request.RestaurantId,
                    Stars = request.Stars,
                    Body = request.Body,
                    CreatedAt = request.CreatedAt,
                };

                user.Reviews.Add(userReview);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}