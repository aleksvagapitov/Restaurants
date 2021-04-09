using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Users.Profiles
{
    public class ProfileReader : IProfileReader
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;
        public ProfileReader(DataContext context, IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
            _context = context;

        }

        public async Task<Profile> ReadProfile(string username)
        {
            var user = await _context.Users.Include(x => x.Photos)
                                            .Include(x => x.Followers)
                                            .Include(x => x.Followings)
                                            .SingleOrDefaultAsync(x => x.UserName == username);

            if (user == null)
                throw new RestException(HttpStatusCode.NotFound, new { User = "Not found" });

            if (user.Role != Role.User)
                throw new RestException(HttpStatusCode.NotFound, new { User = "Not found" });

            var currentUser = await _context.Users.Include(x => x.Followings).SingleOrDefaultAsync(x =>
                x.UserName == _userAccessor.GetCurrentUsername());

            var photos = await _context.Entry(user)
                                        .Collection(x => x.Photos)
                                        .Query()
                                        .ToListAsync();

            var profile = new Profile
            {
                DisplayName = user.DisplayName,
                Username = user.UserName,
                Image = photos.FirstOrDefault(x => x.IsMain)?.Url,
                Photos = photos,
                Biography = user.Biography,
                FollowersCount = user.Followers.Count(),
                FollowingCount = user.Followings.Count()
            };

            if (currentUser.Followings.Any(x => x.TargetId == user.Id))
                profile.IsFollowed = true;
            
            return profile;
        }
    }
}