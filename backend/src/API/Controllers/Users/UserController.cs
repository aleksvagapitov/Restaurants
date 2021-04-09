using System.Threading.Tasks;
using Application.User;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Users
{
    public class UserController : BaseController
    {
        [AllowAnonymous]
        [HttpPost ("login")]
        public async Task<ActionResult<UserDto>> Login (Login.Query query)
        {
            return await Mediator.Send (query);
        }

        [AllowAnonymous]
        [HttpPost ("register")]
        public async Task<ActionResult<UserDto>> Register (Register.Command command)
        {
            return await Mediator.Send (command);
        }

        public async Task<ActionResult<UserDto>> CurrentUser ()
        {
            return await Mediator.Send (new CurrentUser.Query ());
        }
    }
}