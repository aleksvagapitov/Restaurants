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
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Reservations
{
    public class CreateForUser
    {
        public class Command : IRequest
        {
            public Guid RestaurantId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string PhoneNumber { get; set; }
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
                RuleFor(x => x.DateAndTime).NotEmpty().GreaterThanOrEqualTo(DateTime.Now);
                RuleFor(x => x.People).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly IEmailSender _emailSender;
            private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;
            public Handler(DataContext context, IUserAccessor userAccessor,
                            IEmailSender emailSender, IRazorViewToStringRenderer  razorViewToStringRenderer)
            {
                _userAccessor = userAccessor;
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

                var user = await _context.Users.Include(x => x.Reservations)
                                            .SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername());

                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized);

                var userReservation = new Reservation
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber,
                    Email = user.Email,
                    DateTime = request.DateAndTime,
                    People = request.People,
                    Occasion = request.Occasion,
                    SpecialRequest = request.SpecialRequest,
                    IsUser = true,
                    Status = Status.Pending
                };

                restaurant.Reservations.Add(userReservation);
                user.Reservations.Add(userReservation);

                // Telegram/Email The Restaurant

                // Email The User

                var guestEmailDto = new ReservationCreatedForGuestEmailViewModel
                                                {
                                                    RestaurantName = restaurant.Name,
                                                    FirstName = userReservation.FirstName,
                                                    LastName = userReservation.LastName,
                                                    DateAndTime = userReservation.DateTime,
                                                    People = userReservation.People,
                                                    Status = userReservation.Status
                                                };
                    string body = await _razorViewToStringRenderer.RenderViewToStringAsync(EmailTemplate.ReservationCreatedForGuestEmail,  guestEmailDto);
                    var toAddress = new List<string> { userReservation.Email };
                    await _emailSender.SendEmailAsync(toAddress, string.Format("Your Reservation For {0}", guestEmailDto.RestaurantName), body);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}