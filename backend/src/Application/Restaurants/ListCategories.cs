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
    public class ListCategories
    {
        public class Query : IRequest<List<CategoryDto>>
        {
        }

        public class Handler : IRequestHandler<Query, List<CategoryDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<CategoryDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var categoriesQueriable = await _context.RestaurantCategories.AsQueryable().Select(x => x.Category).Distinct().ToListAsync();
                
                var categories = new List<CategoryDto>();

                foreach(var category in categoriesQueriable){
                    categories.Add(new CategoryDto{
                        Text = category,
                        Value = category
                    });
                }
            
                return categories;
            }
        }
    }
}