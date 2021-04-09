using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Security
{
    public class IsRestaurantOwner : IAuthorizationRequirement
    {
        
    }

    public class IsRestaurantOwnerHandler : AuthorizationHandler<IsRestaurantOwner>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _context;
        public IsRestaurantOwnerHandler(IHttpContextAccessor httpContextAccessor, DataContext context)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsRestaurantOwner requirement)
        {
            var currentUserName = _httpContextAccessor.HttpContext.User?.Claims?
                .SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var restaurantId = Guid.Parse(_httpContextAccessor.HttpContext.Request.RouteValues.SingleOrDefault(x => 
                x.Key == "restaurantId").Value.ToString());

            var host = _context.Users.Include(x => x.Restaurants).FirstOrDefault(x => x.UserName == currentUserName);

            if (host.Restaurants.SingleOrDefault(x => x.Id == restaurantId) != null)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}