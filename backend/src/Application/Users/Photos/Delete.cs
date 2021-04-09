using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Users.Photos
{
    public class Delete
    {
        public class Command : IRequest
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly IPhotoAccessor _photoAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor, IPhotoAccessor photoAccessor)
            {
                _photoAccessor = photoAccessor;
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Include(x => x.Photos)
                                .SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername());

                var photo = await _context.Entry(user)
                                        .Collection(x => x.Photos)
                                        .Query()
                                        .FirstOrDefaultAsync(x => x.Id == request.Id);

                if (photo == null)
                    throw new RestException(HttpStatusCode.NotFound, new {Photo = "Not Found"});

                if (photo.IsMain)
                    throw new RestException(HttpStatusCode.BadRequest, new {Photo = "You cannot delete you main photo"});

                var result = _photoAccessor.DeletePhoto(photo.Id);

                // if (result == false)
                //     throw new Exception("Problem deleting the photo");

                user.Photos.Remove(photo);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}