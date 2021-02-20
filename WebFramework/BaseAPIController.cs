using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using WebFramework.Filters;

namespace WebFramework
{
    [ApiController]
    [ApiResultFilter]
    [EnableCors]
    [Route("api/[controller]")]// api/v1/[controller]
    public class BaseAPIController : ControllerBase
    {
        //public UserRepository UserRepository { get; set; } => property injection
        public bool UserIsAutheticated => HttpContext.User.Identity.IsAuthenticated;
    }
}
