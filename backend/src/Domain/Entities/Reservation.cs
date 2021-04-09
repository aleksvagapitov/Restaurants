using System;

namespace Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public Guid RestaurantId { get; set; }
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public string PhoneNumber {get; set;}
        public string Email {get; set;}
        public DateTime DateTime { get; set; }
        public int People { get; set; }
        public string Occasion { get; set; }
        public string SpecialRequest { get; set; }
        public bool IsUser {get; set;}
        public Status Status {get; set;}
    }
}