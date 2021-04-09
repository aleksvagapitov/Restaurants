using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Owners.Restaurants.Photos
{
    public class SetMain
    {
        public class Command : IRequest
        {
            public Guid RestaurantId {get; set;}
            public string PhotoId { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IPhotoAccessor _photoAccessor;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor, IPhotoAccessor photoAccessor)
            {
                _userAccessor = userAccessor;
                _photoAccessor = photoAccessor;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Include(x => x.Photos)
                        .SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername());

                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized);

                var restaurant = await _context.Entry(user)
                                                .Collection(x => x.Restaurants)
                                                .Query()
                                                .Include(x => x.Photos)
                                                .SingleOrDefaultAsync(x => x.Id == request.RestaurantId);

                if (restaurant == null)
                    throw new RestException (HttpStatusCode.NotFound, new { restaurant = "Not found" });

                var photo = await _context.Entry(restaurant)
                                                .Collection(x => x.Photos)
                                                .Query()
                                                .FirstOrDefaultAsync(x => x.Id == request.PhotoId);

                if (photo == null)
                    throw new RestException(HttpStatusCode.NotFound, new {Photo = "Not found"});
                
                var currentMain = await _context.Entry(restaurant)
                                                .Collection(x => x.Photos)
                                                .Query()
                                                .FirstOrDefaultAsync(x => x.IsMain == true);

                currentMain.IsMain = false;
                restaurant.Image = photo.Url;
                photo.IsMain = true;

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}