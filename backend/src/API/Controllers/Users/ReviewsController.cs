using System.Threading.Tasks;
using Application.Users.Reviews;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Users
{
    [Authorize(Roles = Role.User)]
    public class ReviewsController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<Unit>> Add(Add.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(string id)
        {
            return await Mediator.Send(new Delete.Command { Id = id });
        }
    }
}