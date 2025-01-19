using Application.UseCases.Accounts.Command;
using Application.UseCases.Accounts.Query;
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

        [HttpPost]
        [Route("RefreshUserToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshUserToken([FromBody] RefreshTokenCommand refreshTokenCommand)
        {
            var result = await this.mediator.Send(refreshTokenCommand);

            return result.StatusCode == 200 ? Ok(result) : BadRequest(result);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            return await this.mediator.Send(new GetAllUserQuery());
        }


        [HttpPut("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeUserPasswordAsync([FromBody] ChangeUserPassword changeUserPassword)
        {
            return await this.mediator.Send(changeUserPassword);
        }

        [HttpDelete("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserAsync([FromQuery] DeleteUserCommand deleteUserCommand)
        {
            return await this.mediator.Send(deleteUserCommand);
        }
    }
}
