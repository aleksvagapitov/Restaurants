using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public string Biography { get; set; }
        public DateTime UserSince { get; set; }
        public string Role { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<UserFollowing> Followings { get; set; }
        public virtual ICollection<UserFollowing> Followers { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<Restaurant> Restaurants { get; set; }
    }
}