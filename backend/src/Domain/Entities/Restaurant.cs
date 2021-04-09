using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;

namespace Domain.Entities
{
    public class Restaurant
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        [JsonIgnore]
        [Column(TypeName="geography")]
        public Point Location { get; set; }
        public string Description { get; set; }
        public string Image {get; set;}
        public ICollection<RestaurantHours> WorkHours {get; set;}
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<RestaurantCategory> Categories { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}