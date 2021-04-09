using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Users.Profiles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Users
{
    public class ProfilesController : BaseController
    {
        [HttpGet("{username}")]
        public async Task<ActionResult<Profile>> Get (string username)
        {
            return await Mediator.Send(new Details.Query{Username = username});
        }

        [HttpPut]
        public async Task<ActionResult<Unit>> Edit (Edit.Command command)
        {
            return await Mediator.Send(command);
        }
    }
}