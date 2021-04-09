using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using AutoMapper;
using Domain;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Restaurants
{
    public class Details
    {
        public class Query : IRequest<RestaurantDto>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, RestaurantDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<RestaurantDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var restaurant = await _context.Restaurants
                                            .Include(x => x.Categories)
                                            .Include(x => x.Reviews)
                                            .Include(x => x.Photos)
                                            .Include(x => x.WorkHours)
                                            .SingleOrDefaultAsync(x => x.Id == request.Id);

                if (restaurant == null)
                    throw new RestException(HttpStatusCode.NotFound, new { restaurant = "Not found" });

                var photo = await _context.Entry(restaurant)
                                                .Collection(x => x.Photos)
                                                .Query()
                                                .FirstOrDefaultAsync(x => x.IsMain == true);

                
                var restaurantDto = _mapper.Map<Restaurant, RestaurantDto>(restaurant);
                if (photo != null)
                    restaurantDto.Image = photo.Url;

                return restaurantDto;
            }
        }
    }
}