using System;

namespace Application.Restaurants
{
    public class ReviewDto
    {
        public System.Guid Id { get; set; }
        public Guid RestaurantId { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Stars { get; set; }
        public string DisplayName { get; set; }
    }
}