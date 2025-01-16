using Application.UseCases.Accounts.Command;
using Application.UseCases.Users.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RetailingOrderingSystem.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ApiControllerBase
    {
        private IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand registerUserCommand)
        {
            return await this.mediator.Send(registerUserCommand);
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUserAsync(LoginUserCommand loginUserCommand)
        {
            return await this.mediator.Send(loginUserCommand);
        }

        [HttpPut]
        [Route("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] ModifyUserCommand updateUserCommand)
        {
            return await this.mediator.Send(updateUserCommand);
        }
    }
}
