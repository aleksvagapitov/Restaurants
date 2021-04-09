using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Reservations;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Reservations
{
    [Route("api/reservations")]
    public class ReservationsController : BaseController
    {
        [HttpPost ("reserve-as-guest")]
        [AllowAnonymous]
        public async Task<ActionResult<Unit>> ReserveAsGuest (CreateForGuest.Command command)
        {
            return await Mediator.Send (command);
        }

        [HttpPost ("reserve-as-user")]
        public async Task<ActionResult<Unit>> ReserveAsUser (CreateForUser.Command command)
        {
            return await Mediator.Send (command);
        }
    }
}