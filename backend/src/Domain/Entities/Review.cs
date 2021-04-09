using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Review
    {
        public Guid Id { get; set; }
        public Guid RestaurantId { get; set; }
        public string AppUserId { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Stars {get; set;}
        // public virtual ICollection<Photo> Photos { get; set; }
    }
}