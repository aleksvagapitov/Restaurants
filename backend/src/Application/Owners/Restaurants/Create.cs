using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Domain;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Persistence;

namespace Application.Owners.Restaurants
{
    public class Create
    {
        public class Command : IRequest
        {
            public string Name { get; set; }
            public string City { get; set; }
            public string Address { get; set; }
            public string PostalCode { get; set; }
            public string Phone { get; set; }
            public string[] Categories { get; set; }
            public string Description { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.City).NotEmpty();
                RuleFor(x => x.Address).NotEmpty();
                RuleFor(x => x.PostalCode).NotEmpty();
                RuleFor(x => x.Phone).NotEmpty();
                RuleFor(x => x.Categories).NotEmpty();
                RuleFor(x => x.Description).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly UserManager<AppUser> _userManager;
            public readonly ILocationConverter _locationConverter;
            public Handler(UserManager<AppUser> userManager, DataContext context,
                    IUserAccessor userAccessor, ILocationConverter locationConverter)
            {
                _locationConverter = locationConverter;
                _userManager = userManager;
                _userAccessor = userAccessor;
                _context = context;
            }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.Include(x => x.Restaurants)
                                            .ThenInclude(x => x.Categories)
                                            .SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername());

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized);



            var categories = new List<RestaurantCategory>();

            foreach (var item in request.Categories)
            {
                categories.Add(new RestaurantCategory { Category = item });
            }

            var convertedAddress = request.Address.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            var parsedAddress = string.Join("+", convertedAddress) + "+" + request.City;

            var locationInformation = _locationConverter.GetLocationForAddress(parsedAddress).Result;

            var restaurant = new Restaurant
            {
                Name = request.Name,
                City = request.City,
                Address = locationInformation.StreetAddress,
                PostalCode = request.PostalCode,
                Phone = request.Phone,
                Description = request.Description,
                Location = locationInformation.Location,
                Categories = categories
            };

            user.Restaurants.Add(restaurant);

            var success = await _context.SaveChangesAsync() > 0;

            if (success) return Unit.Value;

            throw new Exception("Problem saving changes");
        }
    }
}
}