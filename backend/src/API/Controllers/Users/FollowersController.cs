using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Users.Followers;
using Application.Users.Profiles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Users
{
    [Route("api/profiles")]
    
    public class FollowersController : BaseController
    {
        [HttpPost("{username}/follow")]
        public async Task<ActionResult<Unit>> Follow(string username)
        {
            return await Mediator.Send(new Add.Command{Username = username});
        }

        [HttpDelete("{username}/follow")]
        public async Task<ActionResult<Unit>> UnFollow(string username)
        {
            return await Mediator.Send(new Delete.Command{Username = username});
        }

        [HttpGet("{username}/follow")]
        public async Task<ActionResult<List<Profile>>> GetFollowings(string username, string predicate)
        {
            return await Mediator.Send(new List.Query{Username = username, Predicate = predicate});
        }
    }
}