using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ListReviews
    {
        public class ReviewsEnvelope
        {
            public List<ReviewDto> Reviews { get; set; }
            public int ReviewsCount { get; set; }
        }
        public class Query : IRequest<ReviewsEnvelope>
        {
            public Guid Id { get; set; }
            // public int? Limit { get; set; }
            public int? Offset { get; set; }
        }
        

        public class Handler : IRequestHandler<Query, ReviewsEnvelope>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<ReviewsEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                var restaurant = await _context.Restaurants
                                            .Include(x => x.Reviews)
                                            .SingleOrDefaultAsync(x => x.Id == request.Id);

                if (restaurant == null)
                    throw new RestException(HttpStatusCode.NotFound, new { restaurant = "Not found" });

                var reviewsQueriable = _context.Entry(restaurant)
                                                .Collection(x => x.Reviews)
                                                .Query();

                var reviews = await reviewsQueriable
                    .Where(x => x.RestaurantId == restaurant.Id)
                    .Join(_context.Users,
                        x => x.AppUserId,
                        y => y.Id,
                        (x, y) => new ReviewDto {
                            Id = x.Id,
                            RestaurantId = x.RestaurantId,
                            Body = x.Body,
                            CreatedAt = x.CreatedAt,
                            Stars = x.Stars,
                            DisplayName = y.DisplayName
                    })
                    .Skip(request.Offset ?? 0)
                    .Take(25).ToListAsync();

                return new ReviewsEnvelope
                {
                    Reviews = reviews,
                    ReviewsCount = reviewsQueriable.Count()
                };
            }
        }
    }
}