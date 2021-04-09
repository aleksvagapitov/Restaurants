using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Owners.Restaurants;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Owners
{
    [Authorize(Roles = Role.Owner)]
    public class OwnerRestaurantsController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<List<Restaurant>>> List ()
        {
            return await Mediator.Send (new List.Query());
        }

        [HttpGet("{restaurantId}")]
        [Authorize(Policy = "IsRestaurantOwner")]
        public async Task<ActionResult<Restaurant>> Details(Guid restaurantId)
        {
            return await Mediator.Send(new Details.Query { Id = restaurantId });
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{restaurantId}")]
        [Authorize(Policy = "IsRestaurantOwner")]
        public async Task<ActionResult<Unit>> Edit(Guid restaurantId, Edit.Command command)
        {
            command.Id = restaurantId;
            return await Mediator.Send(command);
        }

        // [HttpDelete("{id}")]
        // public async Task<ActionResult<Unit>> Delete(Guid id)
        // {
        //     return await Mediator.Send(new Delete.Command { Id = id });
        // }
    }
}