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

namespace Application.Admin.Owners
{
    public class Create
    {
        public class Command : IRequest
        {
            public string Email { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly UserManager<AppUser> _userManager;
            private readonly IPasswordGenerator _passwordGenerator;
            private readonly IEmailSender _emailSender;
            private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;

            public Handler(DataContext context, UserManager<AppUser> userManager,
                            IPasswordGenerator passwordGenerator, IEmailSender emailSender, IRazorViewToStringRenderer razorViewToStringRenderer)
            {
                _passwordGenerator = passwordGenerator;
                _userManager = userManager;
                _context = context;
                _emailSender = emailSender;
                _razorViewToStringRenderer = razorViewToStringRenderer;
                
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _context.Users.Where(x => x.Email == request.Email).AnyAsync())
                    throw new RestException(HttpStatusCode.BadRequest, new { Email = "Email already exists" });

                var randomUsername = Guid.NewGuid().ToString();
                while (await _context.Users.Where(x => x.UserName == randomUsername).AnyAsync())
                    randomUsername = Guid.NewGuid().ToString();

                var user = new AppUser
                {
                    UserName = randomUsername,
                    DisplayName = "Owner",
                    UserSince = DateTime.Now,
                    Email = request.Email,
                    Role = Role.Owner
                };

                string password = _passwordGenerator.GeneratePassword();

                var result = await _userManager.CreateAsync(user, password);                
                if (result.Succeeded) {
                    //var emailConfirmationToken = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;
                    var confirmAccountModel = new ConfirmAccountEmailViewModel("http://localhost:3000/", password);
                    string body = await _razorViewToStringRenderer.RenderViewToStringAsync(EmailTemplate.ConfirmAccountEmail,  confirmAccountModel);
                    var toAddress = new List<string> { user.Email };
                    await _emailSender.SendEmailAsync(toAddress, "Confirm your Account", body);

                    return Unit.Value;
                }

                throw new Exception("Problem creating changes");
            }
        }
    }
}