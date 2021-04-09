using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Users.Profiles;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Restaurants
{
    public class List
    {
        public class Query : IRequest<List<RestaurantDto>>
        {
            public Query(DateTime? dateTime, int people, string term)
            {
                SearchDate = dateTime ?? DateTime.Now;
                People = people;
                Term = term;
            }
            public DateTime SearchDate {get; set;}
            public int People {get; set;}
            public string Term { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<RestaurantDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<RestaurantDto>> Handle(Query request, CancellationToken cancellationToken)
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

                if (request.Term != null)
                {
                    var restarauntCategoriesQueriable = restaurantsQueryable.Where(x => x.Categories.Any(x => x.Category.Contains(request.Term)));
                    var restaurantNameQueriable = restaurantsQueryable.Where(x => x.Name.Contains(request.Term));
                    var restarauntCityQueriable = restaurantsQueryable.Where(x => x.City.Contains(request.Term));
                    restaurantsQueryable = restaurantNameQueriable
                                            .Union(restarauntCategoriesQueriable)
                                            .Union(restarauntCityQueriable);
                }

                var restaurants = await restaurantsQueryable.ToListAsync();
                
                var restaurantDto = _mapper.Map<List<Restaurant>, List<RestaurantDto>>(restaurants);

                return restaurantDto;
            }
        }
    }
}