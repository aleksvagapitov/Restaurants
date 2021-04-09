using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using NetTopologySuite.Geometries;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context,
            UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser
                    {
                        Id = "admin",
                        DisplayName = "Admin",
                        UserName = "Admin",
                        Email = "admin@test.com",
                        Role = Role.Admin
                    },
                    new AppUser
                    {
                        Id = "ownerTwoChopsticks",
                        DisplayName = "Owner",
                        UserName = "ownerTwoChopsticks",
                        Email = "ownerTwoChopsticks@test.com",
                        Role = Role.Owner,
                        Restaurants = new List<Restaurant> {
                            new Restaurant {
                                Id = new Guid("7c5e5738-d966-41a2-9418-1ce79dc24827"),
                                Name = "Two Chopsticks",
                                City = "Moscow",
                                Address = "Myasnitskaya 24",
                                PostalCode = "101000",
                                Phone = "805-719-1232",
                                Description = "Now Open: We look forward to serving you soon. We make the finest sushi in town",
                                Location = new Point(37.635206, 55.762667){
                                    SRID = 4326
                                },
                                WorkHours = new List<RestaurantHours>
                                {
                                    new RestaurantHours
                                    {
                                        DayOfWeek = DayOfWeek.Monday,
                                        StartTime = new TimeSpan(0, 0, 0),
                                        EndTime = new TimeSpan(24, 0, 0)
                                    },
                                    new RestaurantHours
                                    {
                                        DayOfWeek = DayOfWeek.Tuesday,
                                        StartTime = new TimeSpan(0, 0, 0),
                                        EndTime = new TimeSpan(24, 0, 0)
                                    },
                                    new RestaurantHours
                                    {
                                        DayOfWeek = DayOfWeek.Wednesday,
                                        StartTime = new TimeSpan(0, 0, 0),
                                        EndTime = new TimeSpan(24, 0, 0)
                                    },
                                    new RestaurantHours
                                    {
                                        DayOfWeek = DayOfWeek.Thursday,
                                        StartTime = new TimeSpan(0, 0, 0),
                                        EndTime = new TimeSpan(24, 0, 0)
                                    },
                                    new RestaurantHours
                                    {
                                        DayOfWeek = DayOfWeek.Friday,
                                        StartTime = new TimeSpan(0, 0, 0),
                                        EndTime = new TimeSpan(24, 0, 0)
                                    }
                                },
                                Categories = new List<RestaurantCategory>
                                {
                                    new RestaurantCategory
                                    {
                                        Category = "Japanese"
                                    },
                                    new RestaurantCategory
                                    {
                                        Category = "Sushi"
                                    },
                                    new RestaurantCategory
                                    {
                                        Category = "Ramen"
                                    }
                                }
                            }
                        }
                    },
                    new AppUser
                    {
                        Id = "ownerSteakHouse",
                        DisplayName = "Owner",
                        UserName = "ownerSteakHouse",
                        Email = "ownerSteakHouse@test.com",
                        Role = Role.Owner,
                        Restaurants = new List<Restaurant> {
                            new Restaurant {
                                Id = new Guid("c8754db9-da96-425e-9ccc-ebc81e91041e"),
                                Name = "New York Steakhouse",
                                City = "Moscow",
                                Address = "Novy Arbat 32",
                                PostalCode = "121099",
                                Description = "Greatest Steakhouse",
                                Phone = "573-849-6384",
                                Location = new Point(37.580661, 55.753311){
                                    SRID = 4326
                                },
                                WorkHours = new List<RestaurantHours>
                                {
                                    new RestaurantHours
                                    {
                                        DayOfWeek = DayOfWeek.Monday,
                                        StartTime = new TimeSpan(0, 0, 0),
                                        EndTime = new TimeSpan(24, 0, 0)
                                    },
                                    new RestaurantHours
                                    {
                                        DayOfWeek = DayOfWeek.Tuesday,
                                        StartTime = new TimeSpan(0, 0, 0),
                                        EndTime = new TimeSpan(24, 0, 0)
                                    },
                                    new RestaurantHours
                                    {
                                        DayOfWeek = DayOfWeek.Wednesday,
                                        StartTime = new TimeSpan(0, 0, 0),
                                        EndTime = new TimeSpan(24, 0, 0)
                                    },
                                    new RestaurantHours
                                    {
                                        DayOfWeek = DayOfWeek.Thursday,
                                        StartTime = new TimeSpan(0, 0, 0),
                                        EndTime = new TimeSpan(24, 0, 0)
                                    },
                                    new RestaurantHours
                                    {
                                        DayOfWeek = DayOfWeek.Friday,
                                        StartTime = new TimeSpan(0, 0, 0),
                                        EndTime = new TimeSpan(24, 0, 0)
                                    }
                                },
                                Categories = new List<RestaurantCategory>
                                {
                                    new RestaurantCategory
                                    {
                                        Category = "Steak"
                                    }
                                }
                            }
                        }
                    },
                    new AppUser
                    {
                        Id = "ownerKoreana",
                        DisplayName = "Owner",
                        UserName = "ownerKoreana",
                        Email = "ownerKoreana@test.com",
                        Role = Role.Owner,
                        Restaurants = new List<Restaurant> {
                            new Restaurant {
                                Id = new Guid("c2ef96f6-9c07-48fe-8c4b-e3501fe732f4"),
                                Name = "Koreana",
                                City = "Saint-Petersburg",
                                Description = "Finest Korean Food",
                                Address = "Gorokhovaya 17",
                                PostalCode = "101000",
                                Phone = "432-567-8364",
                                Location = new Point(30.316102, 59.932171){
                                    SRID = 4326
                                },
                                WorkHours = new List<RestaurantHours>
                                {
                                    new RestaurantHours
                                    {
                                        DayOfWeek = DayOfWeek.Monday,
                                        StartTime = new TimeSpan(0, 0, 0),
                                        EndTime = new TimeSpan(24, 0, 0)
                                    },
                                    new RestaurantHours
                                    {
                                        DayOfWeek = DayOfWeek.Tuesday,
                                        StartTime = new TimeSpan(0, 0, 0),
                                        EndTime = new TimeSpan(24, 0, 0)
                                    },
                                    new RestaurantHours
                                    {
                                        DayOfWeek = DayOfWeek.Wednesday,
                                        StartTime = new TimeSpan(0, 0, 0),
                                        EndTime = new TimeSpan(24, 0, 0)
                                    },
                                    new RestaurantHours
                                    {
                                        DayOfWeek = DayOfWeek.Thursday,
                                        StartTime = new TimeSpan(0, 0, 0),
                                        EndTime = new TimeSpan(24, 0, 0)
                                    },
                                    new RestaurantHours
                                    {
                                        DayOfWeek = DayOfWeek.Friday,
                                        StartTime = new TimeSpan(0, 0, 0),
                                        EndTime = new TimeSpan(24, 0, 0)
                                    }
                                },
                                Categories = new List<RestaurantCategory>
                                {
                                    new RestaurantCategory
                                    {
                                        Category = "Ramen"
                                    }
                                }
                            }
                        }
                    },
                    new AppUser
                    {
                        Id = "ownerGoBistro",
                        DisplayName = "Owner",
                        UserName = "ownerGoBistro",
                        Email = "ownerGoBistro@test.com",
                        Role = Role.Owner,
                        Restaurants = new List<Restaurant> {
                            new Restaurant {
                                Id = new Guid("8767ac7e-4c15-4719-8978-6d06681734f3"),
                                Name = "GoBistro",
                                City = "Hollywood",
                                Address = "2035 Hollywood Blvd",
                                PostalCode = "33020",
                                Phone = "805-719-1232",
                                Description = "Now Open: We look forward to serving you soon. We make the finest sushi in town",
                                Location = new Point(-80.148430, 26.011610){
                                    SRID = 4326
                                },
                                WorkHours = new List<RestaurantHours>
                                {
                                    new RestaurantHours
                                    {
                                        DayOfWeek = DayOfWeek.Monday,
                                        StartTime = new TimeSpan(0, 0, 0),
                                        EndTime = new TimeSpan(24, 0, 0)
                                    },
                                    new RestaurantHours
                                    {
                                        DayOfWeek = DayOfWeek.Tuesday,
                                        StartTime = new TimeSpan(0, 0, 0),
                                        EndTime = new TimeSpan(24, 0, 0)
                                    },
                                    new RestaurantHours
                                    {
                                        DayOfWeek = DayOfWeek.Wednesday,
                                        StartTime = new TimeSpan(0, 0, 0),
                                        EndTime = new TimeSpan(24, 0, 0)
                                    },
                                    new RestaurantHours
                                    {
                                        DayOfWeek = DayOfWeek.Thursday,
                                        StartTime = new TimeSpan(0, 0, 0),
                                        EndTime = new TimeSpan(24, 0, 0)
                                    },
                                    new RestaurantHours
                                    {
                                        DayOfWeek = DayOfWeek.Friday,
                                        StartTime = new TimeSpan(0, 0, 0),
                                        EndTime = new TimeSpan(24, 0, 0)
                                    }
                                },
                                Categories = new List<RestaurantCategory>
                                {
                                    new RestaurantCategory
                                    {
                                        Category = "Japanese"
                                    },
                                    new RestaurantCategory
                                    {
                                        Category = "Sushi"
                                    },
                                    new RestaurantCategory
                                    {
                                        Category = "Ramen"
                                    }
                                }
                            }
                        }
                    },
                    new AppUser
                    {
                        Id = "bob",
                        DisplayName = "Bob",
                        UserName = "bob",
                        Email = "bob@test.com",
                        Role = Role.User,
                        Reviews = new List<Review>
                        {
                            new Review {
                                RestaurantId = new Guid("7c5e5738-d966-41a2-9418-1ce79dc24827"),
                                Stars = 4,
                                Body = "This place is always incredible and always an amazing dining experience! Food is creative. Staff is off the charts and always makes it a focus to ensure you have a beautiful dining experience. Our new go to!!!",
                                CreatedAt = DateTime.Today,
                            },
                            new Review {
                                RestaurantId = new Guid("c8754db9-da96-425e-9ccc-ebc81e91041e"),
                                Stars = 5,
                                Body = "What a great dining experience! We went for lunch recently and sat in their outdoor space, which is covered (good since we went on a rainy day) and has a lot of space",
                                CreatedAt = DateTime.Today
                            },
                            new Review {
                                RestaurantId = new Guid("c2ef96f6-9c07-48fe-8c4b-e3501fe732f4"),
                                Stars = 5,
                                Body = "Fabulous. Always great food, and the effort around safety was clear. Thanks to all!",
                                CreatedAt = DateTime.Today
                            }
                        }
                    },
                    new AppUser
                    {
                        Id = "jane",
                        DisplayName = "Jane",
                        UserName = "jane",
                        Email = "jane@test.com",
                        Role = Role.User,
                        Reviews = new List<Review>
                        {
                            new Review {
                                RestaurantId = new Guid("7c5e5738-d966-41a2-9418-1ce79dc24827"),
                                Stars = 5,
                                Body = "What a great dining experience! We went for lunch recently and sat in their outdoor space, which is covered (good since we went on a rainy day) and has a lot of space",
                                CreatedAt = DateTime.Today
                            },
                            new Review {
                                RestaurantId = new Guid("c8754db9-da96-425e-9ccc-ebc81e91041e"),
                                Stars = 4,
                                Body = "Fabulous. Always great food, and the effort around safety was clear. Thanks to all!",
                                CreatedAt = DateTime.Today
                            },
                            new Review {
                                RestaurantId = new Guid("c2ef96f6-9c07-48fe-8c4b-e3501fe732f4"),
                                Stars = 5,
                                Body = "This place is always incredible and always an amazing dining experience! Food is creative. Staff is off the charts and always makes it a focus to ensure you have a beautiful dining experience. Our new go to!!!",
                                CreatedAt = DateTime.Today
                            }
                        }
                        
                    },
                    new AppUser
                    {
                        Id = "tom",
                        DisplayName = "Tom",
                        UserName = "tom",
                        Email = "tom@test.com",
                        Role = Role.User,
                        Reviews = new List<Review>
                        {
                            new Review {
                                RestaurantId = new Guid("7c5e5738-d966-41a2-9418-1ce79dc24827"),
                                Stars = 4,
                                Body = "Fabulous. Always great food, and the effort around safety was clear. Thanks to all!",
                                CreatedAt = DateTime.Today
                            },
                            new Review {
                                RestaurantId = new Guid("c8754db9-da96-425e-9ccc-ebc81e91041e"),
                                Stars = 5,
                                Body = "This place is always incredible and always an amazing dining experience! Food is creative. Staff is off the charts and always makes it a focus to ensure you have a beautiful dining experience. Our new go to!!!",
                                CreatedAt = DateTime.Today
                            },
                            new Review {
                                RestaurantId = new Guid("c2ef96f6-9c07-48fe-8c4b-e3501fe732f4"),
                                Stars = 4,
                                Body = "What a great dining experience! We went for lunch recently and sat in their outdoor space, which is covered (good since we went on a rainy day) and has a lot of space",
                                CreatedAt = DateTime.Today
                            }
                        }
                    },
                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }
                await context.SaveChangesAsync();
            }
        }
    }
}