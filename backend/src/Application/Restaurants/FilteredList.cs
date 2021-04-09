using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using Persistence;

namespace Application.Restaurants
{
    public class FilteredList
    {
        public class RestaurantsEnvelope
        {
            public List<RestaurantDto> Restaurants { get; set; }
            public int RestaurantCount { get; set; }
        }

        public class Query : IRequest<RestaurantsEnvelope>
        {
            public Query(int? limit, int? offset, DateTime? searchDate, int people, string term, string categories, double latitude, double longitude )
            {
                Limit = limit;
                Offset = offset;
                SearchDate = searchDate ?? DateTime.Now;
                People = people;
                Term = term;
                Categories = categories;
                Latitude = latitude;
                Longitude = longitude;
            }
            public int? Limit { get; set; }
            public int? Offset { get; set; }
            public DateTime SearchDate { get; set; }
            public int People { get; set; }
            public string Term { get; set; }
            public string Categories { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

        public class Handler : IRequestHandler<Query, RestaurantsEnvelope>
        {
            private readonly DataContext _context;
            private readonly ILogger<List> _logger;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, ILogger<List> logger, IMapper mapper, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _logger = logger;
                _context = context;
            }

            public async Task<RestaurantsEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                var restaurantsQueryable = _context.Restaurants
                                            .Include(x => x.Categories)
                                            .Include(x => x.Reviews)
                                            .Include(x => x.Photos)
                                            .Include(x => x.WorkHours)
                                            .AsQueryable();

                TimeSpan time;
                if (request.SearchDate == null)
                    time = new TimeSpan(DateTime.Now.TimeOfDay.Ticks);
                else if (!TimeSpan.TryParse(request.SearchDate.ToString("H:mm"), out time))
                    time = new TimeSpan(DateTime.Now.TimeOfDay.Ticks);

                DateTime date;
                if (request.SearchDate == null)
                    date = DateTime.Now;
                else if (!DateTime.TryParse(request.SearchDate.ToString(), out date))
                    date = DateTime.Now;

                if (date != null && time != null)
                {
                    restaurantsQueryable = restaurantsQueryable.Where(x => x.WorkHours.Any(x => x.DayOfWeek == date.DayOfWeek &&
                                                                        time > x.StartTime && time < x.EndTime));
                }

                if (!String.IsNullOrEmpty(request.Categories))
                    foreach (var category in request.Categories.Split(","))
                        restaurantsQueryable = restaurantsQueryable.Where(x => x.Categories.Any(x => x.Category.Contains(category)));

                if (request.Latitude != 0 && request.Longitude != 0){
                    var currentLocation = new Point(request.Longitude, request.Latitude){
                        SRID = 4326,
                    };
                    restaurantsQueryable = restaurantsQueryable.Where(x => x.Location.Distance(currentLocation) <= 5000);
                }

                if (request.Term != null)
                {
                    var restarauntCategoriesQueriable = restaurantsQueryable.Where(x => x.Categories.Any(x => x.Category.Contains(request.Term)));
                    var restaurantNameQueriable = restaurantsQueryable.Where(x => x.Name.Contains(request.Term));
                    var restarauntCityQueriable = restaurantsQueryable.Where(x => x.City.Contains(request.Term));
                    restaurantsQueryable = restaurantNameQueriable
                                            .Union(restarauntCategoriesQueriable)
                                            .Union(restarauntCityQueriable);
                }

                var restaurants = await restaurantsQueryable
                    .Skip(request.Offset ?? 0)
                    .Take(request.Limit ?? 25).ToListAsync();

                // foreach (var restaurant in restaurants)
                //     var average = Math.Round(restaurant.Reviews.Select(x => x.Stars).Average());



                return new RestaurantsEnvelope
                {
                    Restaurants = _mapper.Map<List<Restaurant>, List<RestaurantDto>>(restaurants),
                    RestaurantCount = restaurantsQueryable.Count()
                };
            }
        }
    }
}