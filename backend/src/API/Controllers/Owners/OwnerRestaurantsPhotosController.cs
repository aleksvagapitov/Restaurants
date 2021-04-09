using System;
using System.Threading.Tasks;
using Application.Owners.Restaurants.Photos;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Owners
{
    [Authorize(Roles = Role.Owner)]
    public class OwnerRestaurantsPhotosController : BaseController
    {
        [HttpPost("{restaurantId}")]
        public async Task<ActionResult<Photo>> Add(Guid restaurantId, [FromForm] Add.Command command)
        {
            command.RestaurantId = restaurantId;
            return await Mediator.Send(command);
        }

        [HttpDelete("{restaurantId}/{photoId}")]
        public async Task<ActionResult<Unit>> Delete(Guid restaurantId, string photoId)
        {
            return await Mediator.Send(new Delete.Command { RestaurantId = restaurantId, PhotoId = photoId });
        }

        [HttpPost("{restaurantId}/{photoId}/setmain")]
        public async Task<ActionResult<Unit>> SetMain(Guid restaurantId, string photoId)
        {
            return await Mediator.Send(new SetMain.Command { RestaurantId = restaurantId, PhotoId = photoId });
        }
    }
}