using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Persistence;

namespace Application.Owners.Restaurants
{
    public class Edit
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string City { get; set; }
            public string Address { get; set; }
            public string PostalCode { get; set; }
            public string Phone { get; set; }
            public string Description {get; set;}
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
                _userManager = userManager;
                _userAccessor = userAccessor;
                _context = context;
                _locationConverter = locationConverter;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                
                var user = await _context.Users.Include(x => x.Restaurants)
                                                .SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername());
                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized);

                var restaurant = await _context.Entry(user)
                                                .Collection(x => x.Restaurants)
                                                .Query()
                                                .SingleOrDefaultAsync(x => x.Id == request.Id);

                if (restaurant == null)
                    throw new RestException (HttpStatusCode.NotFound, new { restaurant = "Not found" });


                var locationInformation = new LocationInformation {
                    StreetAddress = restaurant.Address,
                    Location = restaurant.Location
                };
                if (request.Address != restaurant.Address){
                    var convertedAddress = request.Address.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var parsedAddress = string.Join("+", convertedAddress) + "+" + request.City;
                    locationInformation = _locationConverter.GetLocationForAddress(parsedAddress).Result;
                }

                restaurant.Name = request.Name ?? restaurant.Name;
                restaurant.City = request.City ?? restaurant.City;
                restaurant.Address = locationInformation.StreetAddress;
                restaurant.PostalCode = request.PostalCode ?? restaurant.PostalCode;
                restaurant.Phone = request.Phone ?? restaurant.Phone;
                restaurant.Categories = restaurant.Categories;
                restaurant.Description = restaurant.Description;
                restaurant.Location = locationInformation.Location;
                //Latitude = // Convert Address to Latitude and Longitude

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}