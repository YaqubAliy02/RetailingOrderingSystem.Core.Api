﻿using Application.UseCases.Categories.Query;
using Application.UserCases.Categories.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RetailingOrderingSystem.Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : ApiControllerBase
    {
        private readonly IMediator mediator;

        public CategoryController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("[action]")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategoryAsync(
            [FromBody] CreateCategoryCommand createCategoryCommand)
        {
            var result = await this.mediator.Send(createCategoryCommand);

            return result.StatusCode == 200 ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("[action]")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategoryById([FromQuery] DeleteCategoryCommand deleteCategoryCommand)
        {
            return await this.mediator.Send(deleteCategoryCommand);
        }

        [HttpGet("[action]")]
        // [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetCategoryByIdAsync([FromQuery] GetCategoryByIdQuery getCategoryByIdQuery)
        {
            return await this.mediator.Send(new GetCategoryByIdQuery { Id = getCategoryByIdQuery.Id });
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateCategoryByIdAsync([FromBody] UpdateCategoryCommand updateCategoryCommand)
        {
            return await this.mediator.Send(updateCategoryCommand);
        }

        [HttpGet("[action]")]
        //[Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetAllCategories()
        {
            return await this.mediator.Send(new GetAllCategoriesQuery());
        }
    }
}
