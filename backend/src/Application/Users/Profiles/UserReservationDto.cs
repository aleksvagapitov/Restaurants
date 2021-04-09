using System;
using Domain.Entities;

namespace Application.Users.Profiles
{
    public class UserReservationDto
    {
        public Guid Id { get; set; }
        public Guid RestaurantId { get; set; }
        public string RestaurantName {get; set;}
        public string RestaurantPhoto {get; set;}
        public DateTime DateTime { get; set; }
        public int People { get; set; }
        public string Occasion { get; set; }
        public string SpecialRequest { get; set; }
        public Status Status {get; set;}
    }
}