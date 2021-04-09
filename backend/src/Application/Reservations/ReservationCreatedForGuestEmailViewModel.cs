using System;
using Domain.Entities;

namespace Application.Reservations
{
    public class ReservationCreatedForGuestEmailViewModel
    {
            public string RestaurantName {get; set;}
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime DateAndTime { get; set; }
            public Status Status {get ; set;}
            public int People { get; set; }
    }
}