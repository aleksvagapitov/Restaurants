using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Application.Restaurants
{
    public class RestaurantDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Description {get; set;}
        public string Image {get; set;}
        public ICollection<RestaurantHoursDto> WorkHours {get; set;}
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<RestaurantCategory> Categories {get; set;}
    }
}