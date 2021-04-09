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
    public class Add
    {
        public class Command : IRequest<Photo>
        {
            public Guid RestaurantId { get; set; }
            public IFormFile File { get; set; }
        }

        public class Handler : IRequestHandler<Command, Photo>
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

            public async Task<Photo> Handle(Command request, CancellationToken cancellationToken)
            {
                var photoUploadResult = _photoAccessor.AddPhoto(request.File);

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

                var photos = await _context.Entry(restaurant)
                                                .Collection(x => x.Photos)
                                                .Query()
                                                .ToListAsync();

                var photo = new Photo
                {
                    Url = photoUploadResult.Url,
                    Id = photoUploadResult.PublicId
                };

                if (!photos.Any(x => x.IsMain)){
                    photo.IsMain = true;
                    restaurant.Image = photo.Url;
                }

                restaurant.Photos.Add(photo);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return photo;

                throw new Exception("Problem saving changes");
            }
        }
    }
}