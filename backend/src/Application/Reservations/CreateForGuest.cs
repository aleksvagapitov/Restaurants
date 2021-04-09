using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Reservations
{
    public class CreateForGuest
    {
        public class Command : IRequest
        {
            public Guid RestaurantId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }
            public DateTime DateAndTime { get; set; }
            public int People { get; set; }
            public string Occasion { get; set; }
            public string SpecialRequest { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.FirstName).NotEmpty();
                RuleFor(x => x.LastName).NotEmpty();
                RuleFor(x => x.PhoneNumber).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.DateAndTime).NotEmpty().GreaterThanOrEqualTo(DateTime.Now);
                RuleFor(x => x.People).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IEmailSender _emailSender;
            private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;
            public Handler(DataContext context, IEmailSender emailSender, IRazorViewToStringRenderer  razorViewToStringRenderer)
            {
                _context = context;
                _emailSender = emailSender;
                _razorViewToStringRenderer = razorViewToStringRenderer;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var restaurant = await _context.Restaurants.Include(x => x.Reservations)
                                                            .SingleOrDefaultAsync(x => x.Id == request.RestaurantId);

                if (restaurant == null)
                    throw new Exception("No such Restaurant");

                var guestReservation = new Reservation
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber,
                    Email = request.Email,
                    DateTime = request.DateAndTime,
                    People = request.People,
                    Occasion = request.Occasion,
                    SpecialRequest = request.SpecialRequest,
                    Status = Status.Pending
                };

                restaurant.Reservations.Add(guestReservation);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) {
                    // Telegram/Email The Restaurant
                    var restaurantOwnerEmail = await _context.Users.Include(x => x.Restaurants)
                                                                        .Where(y => y.Restaurants
                                                                            .Any(r => r.Id == request.RestaurantId))
                                                                    .Select(x => x.Email)
                                                                    .FirstOrDefaultAsync();

                    // Email The Guest
                    
                    var guestEmailDto = new ReservationCreatedForGuestEmailViewModel
                                                {
                                                    RestaurantName = restaurant.Name,
                                                    FirstName = guestReservation.FirstName,
                                                    LastName = guestReservation.LastName,
                                                    DateAndTime = guestReservation.DateTime,
                                                    People = guestReservation.People,
                                                    Status = guestReservation.Status
                                                };
                    string body = await _razorViewToStringRenderer.RenderViewToStringAsync(EmailTemplate.ReservationCreatedForGuestEmail,  guestEmailDto);
                    var toAddress = new List<string> { guestReservation.Email };
                    await _emailSender.SendEmailAsync(toAddress, string.Format("Your Reservation For {0}", guestEmailDto.RestaurantName), body);

                    return Unit.Value;
                }

                throw new Exception("Problem saving changes");
            }
        }
    }
}