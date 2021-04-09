using System.Threading.Tasks;
using Application.Admin.Owners;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Admin
{
    [Route("api/admin/owners")]
    [Authorize(Roles = Role.Admin)]
    public class AdminOwnersController : BaseController
    {
        [HttpPost ("create")]
        public async Task<Unit> Create (Create.Command command)
        {
            return await Mediator.Send (command);
        }
    }
}