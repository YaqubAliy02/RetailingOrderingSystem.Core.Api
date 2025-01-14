using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using RetailingOrderingSystem.Core.Api.Filters;

namespace RetailingOrderingSystem.Core.Api.Controllers
{
    [ApiController]
    [ValidationActionFilter]
    public class ApiControllerBase : ControllerBase
    {
        private readonly IMapper mapper;

        protected IMapper Mapper => this.mapper ?? HttpContext
            .RequestServices.GetRequiredService<IMapper>();
    }
}   
