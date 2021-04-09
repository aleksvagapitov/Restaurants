using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Users.Profiles;
using Application.Users.Reservations;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Users
{
    [Route("api/profile/reservations")]
    [Authorize(Roles = Role.User)]
    public class UserReservationsController : BaseController
    {
        public async Task<ActionResult<List<UserReservationDto>>> GetUserReservations(string predicate)
        {
            return await Mediator.Send(new ListReservations.Query{Predicate = predicate});
        }
    }
}