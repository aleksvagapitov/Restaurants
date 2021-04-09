using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Owners.Reservations;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Owners
{
    [Authorize(Roles = Role.Owner)]
    public class OwnerReservationsController : BaseController
    {
        [HttpGet("{restaurantId}")]
        [Authorize(Policy = "IsRestaurantOwner")]
        public async Task<ActionResult<List<Reservation>>> List(Guid restaurantId)
        {
            return await Mediator.Send(new List.Query { RestaurantId = restaurantId });
        }

        [HttpPut("{restaurantId}/{reservationId}")]
        [Authorize(Policy = "IsRestaurantOwner")]
        public async Task<ActionResult<Unit>> Edit(Guid restaurantId, Guid reservationId, Edit.Command command)
        {
            command.ReservationId = reservationId;
            command.RestaurantId = restaurantId;
            return await Mediator.Send(command);
        }

    }
}