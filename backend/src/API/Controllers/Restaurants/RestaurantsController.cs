using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Restaurants;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Application.Restaurants.ListReviews;

namespace API.Controllers.Restaurants
{
    [Route("api/restaurants")]
    [AllowAnonymous]
    public class RestaurantController : BaseController
    {

        [AllowAnonymous]
        public async Task<ActionResult<List<RestaurantDto>>> List(DateTime? dateTime, int people, string term)
        {
            return await Mediator.Send(new List.Query(dateTime, people, term));
        }

        [AllowAnonymous]
        [HttpGet("filtered")]
        public async Task<ActionResult<FilteredList.RestaurantsEnvelope>> FilteredList(int? limit, int? offset, 
            DateTime? searchDate, int people, string term, string categories, double latitude, double longitude)
        {
            return await Mediator.Send(new FilteredList.Query(limit, offset, searchDate, 
                                                                people, term, categories,
                                                                latitude, longitude));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantDto>> Details(Guid id)
        {
            return await Mediator.Send(new Details.Query { Id = id });
        }

        [AllowAnonymous]
        [HttpGet ("categories")]
        public async Task<ActionResult<List<CategoryDto>>> ListCategories()
        {
            return await Mediator.Send(new ListCategories.Query());
        }

        [AllowAnonymous]
        [HttpGet ("{id}/reviews/{offset}")]
        public async Task<ActionResult<ReviewsEnvelope>> ListReviews(Guid id, int? offset)
        {
            return await Mediator.Send(new ListReviews.Query{Id = id, Offset = offset});
        }
    }
}