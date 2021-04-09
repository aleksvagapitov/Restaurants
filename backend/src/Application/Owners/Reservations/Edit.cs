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
using Persistence;

namespace Application.Owners.Reservations
{
    public class Edit
    {
        public class Command : IRequest
        {
            public Guid ReservationId { get; set; }
            public Guid RestaurantId { get; set; }
            public Status Status {get; set;}
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                // RuleFor(x => x.Status).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly UserManager<AppUser> _userManager;
            private readonly IEmailSender _emailSender;
            private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;
            public Handler(UserManager<AppUser> userManager, DataContext context, IUserAccessor userAccessor, 
                            IEmailSender emailSender, IRazorViewToStringRenderer razorViewToStringRenderer)
            {
                _userManager = userManager;
                _userAccessor = userAccessor;
                _context = context;
                _emailSender = emailSender;
                _razorViewToStringRenderer = razorViewToStringRenderer;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Include(x => x.Restaurants)
                                                    .ThenInclude(x => x.Reservations)
                                                .SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername());
                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized);

                var restaurant = await _context.Entry(user)
                                                .Collection(x => x.Restaurants)
                                                .Query()
                                                .SingleOrDefaultAsync(x => x.Id == request.RestaurantId);
                if (restaurant == null)
                    throw new RestException (HttpStatusCode.NotFound, new { restaurant = "Not found" });

                var reservation = await _context.Entry(restaurant)
                                                .Collection(x => x.Reservations)
                                                .Query()
                                                .SingleOrDefaultAsync(x => x.Id == request.ReservationId);
                if (reservation == null)
                    throw new RestException (HttpStatusCode.NotFound, new { reservation = "Not found" });
                
                reservation.Status = request.Status;

                // Save
                var success = await _context.SaveChangesAsync() > 0;

                if (success) {
                    var reservationEditedDto = new ReservationEditedByOwnerViewModel
                                                {
                                                    RestaurantName = restaurant.Name,
                                                    FirstName = reservation.FirstName,
                                                    LastName = reservation.LastName,
                                                    DateAndTime = reservation.DateTime,
                                                    People = reservation.People,
                                                    Status = reservation.Status.ToString()
                                                };
                    string body = await _razorViewToStringRenderer.RenderViewToStringAsync(EmailTemplate.ReservationEditedByOwnerEmail,  reservationEditedDto);
                    var toAddress = new List<string> { reservation.Email };
                    await _emailSender.SendEmailAsync(toAddress, string.Format("Your {0} reservation has been {1}", 
                                                                reservationEditedDto.RestaurantName, reservation.Status.ToString()),body);

                    return Unit.Value;
                }

                throw new Exception("Problem saving changes");
            }
        }
    }
}