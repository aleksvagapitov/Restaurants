using System;

namespace Application.Owners.Reservations
{
    public class ReservationEditedByOwnerViewModel
    {
        public string RestaurantName {get; set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateAndTime { get; set; }
        public int People { get; set; }
        public string Status { get; set; }
    }
}